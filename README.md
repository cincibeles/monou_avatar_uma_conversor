# monou_avatar_uma_conversor
Proyecto Unity para cargar y transformar modelos de monou avatares a format UMA 

### Instalación
- Descargar este repositorio
- Abrir este repo en Unity
- Instalar Paquete UMA https://assetstore.unity.com/packages/3d/characters/uma-2-unity-multipurpose-avatar-35611
- Insertar en la escena el asset Assets/UMA/Getting Started/Deprecated/UMA_DCS
- Insertar en la escena el asset Assets/UMA/Getting Started/UMADynamicCharacterAvatar.prefab
- Abrir la ventana "Global Library" en UMA>Global Library
- Arrastrar la carpeta Assets/Monou Avatar al primer cuadro inferior izquierdo "Drag Indenxable Assets..."
- Seleccionar el objeto en escena UMADynamicCharacterAvatar y cambiar su atributo "Active Race" a el valor "Monou"
- Para asignarle acesorios, de las subcarpetas de la carpeta "Asset/Monou Avatar" arrastras los archivos terminación "Recipe.asset" (icono de casco) al objeto en escena UMADynamicCharacterAvatar en su aprtado "customization" a la caja "Drag Wardribe Recipes...".

### Creación y uso de los paquetes AssetBundle
- Abrir la ventana UMA assetBundle: UMA > UMA AssetBundle Manager
- Oprimir el botón "Rebuild AssetBundles" para general los AssetBunbdles.
- Marcar la casilla "Star Server" para iniciar un servidor virtual para despachar los assetBundles

### Tutoriales para adaptar los assets al formati UMA
]Importante. Dado que se usarán AssetBunlde separados por prenda, no usar el mismo material como es sugerido en los videos.
- Creación de una Raza https://www.youtube.com/watch?v=KGgyv3CSWek
- Creación de un Asset https://www.youtube.com/watch?v=7wklwssOWB4
