(function () {
    "use strict";

    // The Office initialize function must be run each time a new page is loaded
    Office.initialize = function (reason) {
        $(document).ready(function () {

            Directory();
        });
    };

    function Directory() {
        document.getElementById("openFolder").onclick = async () => {
            const directoryHandle = await window.showDirectoryPicker();

            console.log(directoryHandle.name);
            DisplaySelectedDirectory(directoryHandle.name);
            console.log("DONNNE");

            for await (const entry of directoryHandle.values()) {
                console.log(entry.kind, entry.name);
            }
        };
    }

    function DisplaySelectedDirectory(name) {
        document.getElementById("folderChosen").innerHTML = name;
    }



})();

/*document.querySelector("openFolder").onclick = async () => {
    const directoryHandle = await window.showDirectoryPicker();

    for await (const entry of directoryHandle.values()) {
        console.log(entry.kind, entry.name);
    }
};*/

/*document.getElementById("openFolder").onclick = async () => {
    const directoryHandler = await window.showDirectoryPicker();

    console.log(opened);
    for await (const entry of directoryHandle.values()) {
        console.log(entry.kind, entry.name);
    }

};*/

