using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Globalization;
using UMA;
using UMA.CharacterSystem;


namespace Monou
{
    public class MonouParticles : MonoBehaviour
    {
        public List<DynamicCharacterAvatar> dcas = new List<DynamicCharacterAvatar>();

        private Dictionary<string, MaterialParticle> materialParticles = new Dictionary<string, MaterialParticle>();
        private int timeCounter = 0;
        private Mesh plane;

        // Start is called before the first frame update
        void Start()
        {
            createPlane();
            if(dcas.Count == 0){
                DynamicCharacterAvatar[] _dcas = GameObject.FindObjectsOfType<DynamicCharacterAvatar>();
                foreach(DynamicCharacterAvatar dca in _dcas) dcas.Add(dca);
            }
            StartCoroutine(updateParticleMaterialOffset());
        }

        // Update is called once per frame
        void Update()
        {
            searchBehabior();
            drawParticles();
        }

        private void searchBehabior(){
            string[] blocks;
            string[] split;
            string type;
            string[] data;
            for (int i=dcas.Count-1; i>=0; i--){
                foreach (UMAWardrobeRecipe uwr in dcas[i].WardrobeRecipes.Values){
                    if(uwr.UserField.Length>0){
                        blocks = uwr.UserField.Split(';');
                        foreach(string block in blocks){
                            split = block.Split(':');
                            type = split[0];
                            data = split[1].Split(',');
                            prepare(type, data, dcas[i]);
                        }
                    }
                }
                dcas.RemoveAt(i);
            }
        }

        private void prepare(string type, string[] data, DynamicCharacterAvatar avatar){
            if(type=="particle") createParticle(data, avatar);
        }

        public void createParticle(string[] data, DynamicCharacterAvatar avatar){
            string materialName = "";
            string parentName = "";
            Vector3 position = new Vector3();
            Vector2 size = new Vector2();
            if(data.Length>0) materialName = data[0];
            if(data.Length>1) parentName = data[1];
            if(data.Length>4) position = new Vector3(float.Parse(data[2]), float.Parse(data[3]), float.Parse(data[4]));
            if(data.Length>6) size = new Vector2(float.Parse(data[5]), float.Parse(data[6]));
            Transform parent = RecursiveFindChild(avatar.gameObject.transform, parentName);
            print(avatar.gameObject);
            if(parent == null) return;
            Material m = (Material)Resources.Load(materialName, typeof(Material));
            if(m == null) return;
            if(!materialParticles.ContainsKey(materialName)) materialParticles[materialName] = new MaterialParticle(m);
            materialParticles[materialName].particles.Add(new Particle(parent, position, size, materialName));
        }

        IEnumerator updateParticleMaterialOffset() {
            do{
                yield return new WaitForSeconds(.05F);
                foreach(KeyValuePair<string, MaterialParticle> item in materialParticles) {
                    Vector2 scale = item.Value.material.GetTextureScale("_MainTex");
                    int cols = (int) Mathf.Round(1/scale.x);
                    int rows = (int) Mathf.Round(1/scale.y);
                    int totalFrames = (int) Mathf.Floor(cols * rows);
                    int i = (int) timeCounter % totalFrames;
                    float x = (i%cols);
                    float y = Mathf.Floor(i/cols);
                    print("anim "+cols.ToString()+", "+rows.ToString()+", "+totalFrames.ToString()+"; "+i.ToString()+", "+x.ToString()+", "+y.ToString());
                    item.Value.material.SetTextureOffset("_MainTex", new Vector2(x*scale.x, 1-y*scale.y));
                }
                timeCounter++; if(timeCounter>1000000) timeCounter=0;
            }while(true); 
        }

        private void createPlane(){
            plane = new Mesh();
            plane.vertices = new Vector3[4] { new Vector3(0.5f,0.5f,0), new Vector3(0.5f,-0.5f,0), new Vector3(-0.5f,-0.5f,0), new Vector3(-0.5f,0.5f,0) };
            plane.colors = new Color[4]{ Color.white, Color.white, Color.white, Color.white };
            plane.uv = new Vector2[4] { new Vector2(1,1), new Vector2(1,0), new Vector2(0,0), new Vector2(0,1) };
            plane.triangles = new int[6]{0,2,1,0,3,2};
            plane.RecalculateBounds();
            plane.RecalculateNormals();
        }

        private void drawParticles(){
            Matrix4x4 m = Camera.main.cameraToWorldMatrix;
            m.SetColumn(0, m.GetColumn(0)*-1); m.SetColumn(2, m.GetColumn(2)*-1); // rota 180 grados
            Vector4 axisX = m.GetColumn(0);
            Vector4 axisY = m.GetColumn(1);
            Vector4 axisZ = m.GetColumn(2);
            
            foreach(KeyValuePair<string, MaterialParticle> item in materialParticles) {
                List<Matrix4x4> batches = new List<Matrix4x4>();
                foreach(Particle p in item.Value.particles){
                    Vector4 newAxisX = axisX * p.size.x; // define la escala en X
                    Vector4 newAxisY = axisY * p.size.y; // define la escala en Y
                    Matrix4x4 parentMatrix = p.parent.transform.localToWorldMatrix;
                    Vector4 parentX = Vector4.Normalize(parentMatrix.GetColumn(0)) * p.position.x;
                    Vector4 parentY = Vector4.Normalize(parentMatrix.GetColumn(1)) * p.position.y;
                    Vector4 parentZ = Vector4.Normalize(parentMatrix.GetColumn(2)) * p.position.z;
                    Vector4 parentPos = parentMatrix.GetColumn(3) + parentX + parentY + parentZ;
                    batches.Add(new Matrix4x4( newAxisX,newAxisY,axisZ, parentPos));
                }
                Graphics.DrawMeshInstanced(plane, 0, item.Value.material, batches);
            }
        }

        private Transform RecursiveFindChild(Transform parent, string childName){
            foreach (Transform child in parent){
                if(child.name == childName) return child;
                else {
                    Transform found = RecursiveFindChild(child, childName);
                    if (found != null) return found;
                }
            }
            return null;
        }
    }

    public class Particle {
        public Transform parent;
        public Vector3 position;
        public Vector2 size;
        public string materialName;
        public Particle(Transform got, Vector3 pos, Vector2 sz, string mName){
            parent = got;
            position = pos;
            size = sz;
            materialName = mName;
        }
    }
    public class MaterialParticle {
        public Material material;
        public List<Particle> particles;
        public MaterialParticle(Material m){
            material = m;
            particles = new List<Particle>();
        }
    }
}