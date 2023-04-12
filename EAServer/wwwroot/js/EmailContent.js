///RETAG BUTTON///
function Retag() {
    document.getElementById("save").onclick = async () => {
        console.log("retagging");
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
                             AssignTags(res.data);
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