/*Office.initialize = function () {
}
*/

/*Office.initialize = function () {
    $(document).ready(function () {

       console.log("HELLO THERE")
    });
};*/
Office.initialize = function () {
    console.log("HELLO THERE")
/*    // Register event handler for the button click
    var button = document.getElementById("myButton");
    button.onclick = onButtonClick;*/
};

function addInformational() {
    const details = {
        type: Office.MailboxEnums.ItemNotificationMessageType.InformationalMessage,
        message: "Tagging email...",
        icon: "icon1",
        persistent: false
    };
    Office.context.mailbox.item.notificationMessages.addAsync("notification", details, handleResult);
}

// Helper function to add a status message to the info bar.
function statusUpdate(icon, text) {
  Office.context.mailbox.item.notificationMessages.replaceAsync("status", {
    type: "informationalMessage",
    icon: icon,
    message: text,
    persistent: false
  });
}

function defaultStatus(event) {
  statusUpdate("icon16" , "Hello World!");
}

async function getFrom() {
    //Get the from and append the client's name
    const msgFrom = Office.context.mailbox.item.from;
    var fromField = msgFrom.displayName;

    //Get the subject and append it
    var subjectField = Office.context.mailbox.item.subject;

    console.log("Got subject and from");

    // Call displayNotification to show progress
    await displayNotification();

    await Office.context.mailbox.item.body.getAsync(
        "text",
        function (result) {
            if (result.status === Office.AsyncResultStatus.Succeeded) {
                var bodyField = result.value;

                console.log(bodyField);

                axios.get("/Home/getTag", {
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

                        // Register an event handler that displays the notification message
                        Office.context.mailbox.addHandlerAsync(Office.EventType.ItemChanged, function (event) {
                            displayNotification().then(function () {
                                event.completed();
                            }).catch(function (error) {
                                console.error(error);
                                event.completed();
                            });
                        });
                       
                    });
            }
            else {
                console.log(result.status);
            }
        }
    )
}

function displayNotification() {
    // Create a new progressIndicator notification message
    Office.context.mailbox.item.notificationMessages.addAsync('notification', {
        type: 'progressIndicator',
        message: 'My add-in is working on your request...',
       // icon: 'icon-16'
    });

    // Update the progress value as the operation progresses
    var progress = 0;
    var intervalId = setInterval(function () {
        progress += 10;
        if (progress <= 100) {
            Office.context.mailbox.item.notificationMessages.updateAsync('notification', {
                progressIndicator: {
                    percentComplete: progress
                }
            });
        } else {
            clearInterval(intervalId);
        }
    }, 1000);

    // Wrap the notification message code in a Promise object
    return new Promise(function (resolve, reject) {
        // Perform some asynchronous operation here
        // ...

        // Remove the notification message when the operation is complete
        clearInterval(intervalId);
        Office.context.mailbox.item.notificationMessages.removeAsync('notification', function (result) {
            if (result.status === Office.AsyncResultStatus.Succeeded) {
                resolve();
            } else {
                reject(new Error('Failed to remove notification message.'));
            }
        });
    });
}



function handleResult(result) {
    console.log(result);
    event.completed();
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
                       // alert("We've scanned through thousands of emails");
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