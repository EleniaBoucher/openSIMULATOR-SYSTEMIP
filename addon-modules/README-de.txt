In diesem Verzeichnis können Sie Addon-Module für OpenSim platzieren

Jedes Modul sollte in seinem eigenen Baum und die Wurzel des Baumes sein
Sollte eine Datei mit dem Namen "prebuild.xml" enthalten, die in der
Hauptvorbau-Datei.

Die Prebuild.xml sollte nur <Project> und zugehörige untergeordnete Tags enthalten.
Die Tags <? Xml>, <Prebuild>, <Solution> und <Configuration> sollten nicht sein
Enthalten, da die Add-on-Module prebuild.xml direkt in die Haupt-Prebuild.xml eingefügt wird
