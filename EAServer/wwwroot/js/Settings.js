(function () {
    "use strict";

    // The Office initialize function must be run each time a new page is loaded
    Office.initialize = function (reason) {
        $(document).ready(function () {
            //Retag();
            Override(); // buttons at the same time.
            //sendData();
            //ImportantWordType();
            //BackButtonClick();
            // AddButtonClick();
        });
    };

    async function sendData() {
        getFrom();
    }

    async function getFrom() {
        //Get the from and append the client's name
        const msgFrom = Office.context.mailbox.item.from;
        var fromField = msgFrom.displayName;

        //Get the subject and append it
        var subjectField = Office.context.mailbox.item.subject;

        console.log("Got subject and from");

        await Office.context.mailbox.item.body.getAsync(
            "text",
            function (result) {
                if (result.status === Office.AsyncResultStatus.Succeeded) {
                    var bodyField = result.value;

                    console.log(bodyField);

                    axios.get("/Home/testing", {
                        params:
                        {
                            from: fromField,
                            subject: subjectField,
                            body: bodyField
                        }
                    })
                        .then(res => {
                            console.log(res.data);
                            AssignTags(res.data);
                        });
                }
                else {
                    console.log(result.status);
                }
            }
        )
    }

    //Assign the tag colors to the email
    // Note: In order for you to successfully add a category, it must be in the mailbox categories master list.
    function AssignTags(tagColor) {

        Office.context.mailbox.masterCategories.getAsync(function (asyncResult) {
            if (asyncResult.status === Office.AsyncResultStatus.Succeeded) {
                const masterCategories = asyncResult.value;
                if (masterCategories && masterCategories.length > 0) {
                    const categoryToAdd = [tagColor];
                    Office.context.mailbox.item.categories.addAsync(categoryToAdd, function (asyncResult) {
                        if (asyncResult.status === Office.AsyncResultStatus.Succeeded) {
                            console.log(`Successfully assigned category '${categoryToAdd}' to item.`);
                            alert("We've scanned through thousands of emails");
                        } else {
                            console.log("categories.addAsync call failed with error: " + asyncResult.error.message);
                        }
                    });
                } else {
                    console.log(
                        "There are no categories in the master list on this mailbox. You can add categories using Office.context.mailbox.masterCategories.addAsync."
                    );
                }
            } else {
                console.error(asyncResult.error);
            }
        });
    }

    //Get the list of available tag colors and check if it's already in the master category list. If not add it.
    function getMasterCategories() {

        const map = new Map();
        map.set("High Priority", false);
        map.set("Medium Priority", false);
        map.set("Low Priority", false);

        Office.context.mailbox.masterCategories.getAsync(function (asyncResult) {
            if (asyncResult.status === Office.AsyncResultStatus.Succeeded) {
                const categories = asyncResult.value;
                if (categories && categories.length > 0) {

                    for (var i = 0; i < categories.length; i++) {
                        if (categories[i].displayName === "High Priority") {
                            map.set(categories[i].displayName, true);
                        }
                        else if (categories[i].displayName === "Medium Priority") {
                            map.set(categories[i].displayName, true);
                        }
                        else if (categories[i].displayName === "Low Priority") {
                            map.set(categories[i].displayName, true);
                        }
                    }

                } else {
                    console.log("There are no categories in the master list.");
                }
            } else {
                console.error(asyncResult.error);
            }

            for (var i = 0; i < map.size; i++) {
                if (map.get("High Priority") === false) {
                    addMasterCategories("High Priority", Office.MailboxEnums.CategoryColor.Preset0);
                    map.set(categories[i].displayName, true);
                }
                else if (map.get("Medium Priority") === false) {
                    addMasterCategories("Medium Priority", Office.MailboxEnums.CategoryColor.Preset3);
                    map.set(categories[i].displayName, true);
                }
                else if (map.get("Low Priority") === false) {
                    addMasterCategories("Low Priority", Office.MailboxEnums.CategoryColor.Preset4)
                    map.set(categories[i].displayName, true);
                }
            }
        });
    }

    //Adds the tag colors to the master category
    function addMasterCategories(name, tagColor) {
        const masterCategoriesToAdd = [
            {
                displayName: name,
                color: tagColor
            }
        ];

        Office.context.mailbox.masterCategories.addAsync(masterCategoriesToAdd, function (asyncResult) {
            if (asyncResult.status === Office.AsyncResultStatus.Succeeded) {
            } else {
                console.log("masterCategories.addAsync call failed with error: " + asyncResult.error.message);
            }
        });
    }

    //Get the tag colors that have been set on the email. 
    //NOTE: I was thinking of using this method to move the emails into the designated folders
    function getCategories() {
        Office.context.mailbox.item.categories.getAsync(function (asyncResult) {
            if (asyncResult.status === Office.AsyncResultStatus.Succeeded) {
                const categories = asyncResult.value;
                if (categories && categories.length > 0) {
                    console.log("Categories assigned to this item:");
                    console.log(JSON.stringify(categories));
                } else {
                    console.log("There are no categories assigned to this item.");
                }
            } else {
                console.error(asyncResult.error);
            }
        });
    }

    //Add the tags to the email
    // Note: In order for you to successfully add a category,
    // it must be in the mailbox categories master list.
    function addCategories() {
        Office.context.mailbox.masterCategories.getAsync(function (asyncResult) {
            if (asyncResult.status === Office.AsyncResultStatus.Succeeded) {
                const masterCategories = asyncResult.value;
                if (masterCategories && masterCategories.length > 0) {
                    // Grab the first category from the master list.
                    //const categoryToAdd = [masterCategories[0].displayName];
                    const categoryToAdd = ["High Priority"];
                    Office.context.mailbox.item.categories.addAsync(categoryToAdd, function (asyncResult) {
                        if (asyncResult.status === Office.AsyncResultStatus.Succeeded) {
                            console.log(`Successfully assigned category '${categoryToAdd}' to item.`);
                        } else {
                            console.log("categories.addAsync call failed with error: " + asyncResult.error.message);
                        }
                    });
                } else {
                    console.log(
                        "There are no categories in the master list on this mailbox. You can add categories using Office.context.mailbox.masterCategories.addAsync."
                    );
                }
            } else {
                console.error(asyncResult.error);
            }
        });
    }

    function addWordWeightsIntial() {
        //Get the div
        var container = document.getElementById("addWordWeights");

        container.innerHTML = ` <div>
            <p> Important words </p>
            <textarea class="importantwords" rows="1" cols="10"> </textarea>
            <p> Weight </p>
            </div>`

        //Create the dropdown and add elements to it
        var select = document.createElement("select");
        for (var i = 1; i <= 100; i++) {
            var option = document.createElement("option");
            option.value = i;
            option.text = i;
            select.appendChild(option);
        }

        container.appendChild(select);
    }

    function AddButtonClick() {
        document.getElementById("addBtn").onclick = async () => {
            //Get the div
            var container = document.getElementById("addWordWeights");

            container.innerHTML += ` <div>
            <p> Important words </p>
            <textarea class="importantwords" rows="1" cols="10"> </textarea>
            <p> Weight </p>
            </div>`

            //Create the dropdown and add elements to it
            var select = document.createElement("select");
            for (var i = 1; i <= 100; i++) {
                var option = document.createElement("option");
                option.value = i;
                option.text = i;
                select.appendChild(option);
            }

            container.appendChild(select);
        }
    }

    function ImportantWordType() {
        document.getElementById("submitBtn").onclick = async () => {

            const companyNameSelected = document.getElementById('companyName').checked;

            if (companyNameSelected) {
                document.getElementById("companyNameResponse").style.display = "block";
            }
            else {
                document.getElementById("clientNameResponse").style.display = "block";
            }

            //Add the word weight dropdown
            addWordWeightsIntial();
            document.getElementById("addWordWeights").style.display = "block"
            document.getElementById("questionAndResponse").style.display = "none"
            document.getElementById("back").style.display = "block"
            document.getElementById("addBtn").style.display = "block"
        }
    }

    function BackButtonClick() {
        document.getElementById("back").onclick = async () => {

            //Hide the buttons and the divs
            document.getElementById("back").style.display = "none"
            document.getElementById("companyNameResponse").style.display = "none";
            document.getElementById("clientNameResponse").style.display = "none";
            document.getElementById("addWordWeights").style.display = "none";
            document.getElementById("addBtn").style.display = "none";

            //document.getElementById("backClick").style.display = "none";
            document.getElementById("questionAndResponse").style.display = "block"
        }
    }
    ///RETAG BUTTON///
    function Retag() {
        document.getElementById("save").onclick = async () => {
            var tagColor = "";

            removeCategories();
            if (document.getElementById("low").checked) {
                AssignTags("Low Priority");
                tagColor = "Low Priority";
            }
            else if (document.getElementById("medium").checked) {
                AssignTags("Medium Priority");
                tagColor = "Medium Priority";
            }
            else {
                AssignTags("High Priority");
                tagColor = "High Priority";
            }
            await Office.context.mailbox.item.body.getAsync(
                "text",
                function (result) {
                    if (result.status === Office.AsyncResultStatus.Succeeded) {
                        var bodyField = result.value;

                        console.log(bodyField);

                        axios.get("/Home/Retag", {
                            params:
                            {
                                body: bodyField,
                                tag: tagColor
                            }
                        })
                            .then(res => {
                                console.log(res.data);
                                // AssignTags(res.data);
                            });
                    }
                    else {
                        console.log(result.status);
                    }
                }
            )

        }
    }
    ///Override BUTTON///
    function Override() {
        document.getElementById("sendersave").onclick = async () => {
            var tagColor = "";

            removeCategories();
            if (document.getElementById("senderlow").checked) {
                AssignTags("Low Priority");
                tagColor = "Low Priority";
            }
            else if (document.getElementById("sendermedium").checked) {
                AssignTags("Medium Priority");
                tagColor = "Medium Priority";
            }
            else if (document.getElementById("senderRemove").checked) {
                tagColor = "remove";
            }
            else {
                AssignTags("High Priority");
                tagColor = "High Priority";
            }
            const msgFrom = Office.context.mailbox.item.from;
            var fromField = msgFrom.displayName;
            axios.get("/Home/Override", {
                params:
                {
                    sender: fromField,
                    tag: tagColor
                }
            })
                .then(res => {
                    console.log(res.data);
                    // AssignTags(res.data);
                });
        }
    }

    function removeCategories() {
        Office.context.mailbox.item.categories.getAsync(function (asyncResult) {
            if (asyncResult.status === Office.AsyncResultStatus.Succeeded) {
                const categories = asyncResult.value;
                if (categories && categories.length > 0) {
                    // Grab the first category assigned to this item.
                    const categoryToRemove = [categories[0].displayName];
                    Office.context.mailbox.item.categories.removeAsync(categoryToRemove, function (asyncResult) {
                        if (asyncResult.status === Office.AsyncResultStatus.Succeeded) {
                            console.log("Successfully unassigned category" + categoryToRemove + "from this item.");
                        } else {
                            console.log("categories.removeAsync call failed with error: " + asyncResult.error.message);
                        }
                    });
                } else {
                    console.log("There are no categories assigned to this item.");
                }
            } else {
                console.error(asyncResult.error);
            }
        });
    }

})();