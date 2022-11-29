# KSaveDataManager
Sava/Load simple to complex class data for unity engine. Very early prototype stage prototype designed to load and save data from disk or cloud with encryption support. Supported unity version: 2020.3.3f1 or up

#### Installation:
* Add an entry in your manifest.json as follows:
```C#
"com.kaiyum.ksavedatamanager": "https://github.com/kaiyumcg/KSaveDataManager.git"
```

Since unity does not support git dependencies, you need the following entries as well:
```C#
"com.kaiyum.attributeext" : "https://github.com/kaiyumcg/AttributeExt.git",
"com.kaiyum.unityext": "https://github.com/kaiyumcg/UnityExt.git"
```
Add them into your manifest.json file in "Packages\" directory of your unity project, if they are already not in manifest.json file.