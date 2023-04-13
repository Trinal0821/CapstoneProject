Office.initialize = function () {
    console.log("Hello")
  //  downloadEmails();
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
    statusUpdate("icon16", "Hello World!");
}

async function tagEmail() {
    // Ensure the Firebase SDK has been initialized before continuing.
    firebase.initializeApp({
        apiKey: "AIzaSyDVBPzpvxoABeSMrz_MKhSuO-b6BZm8MgA",
        authDomain: "executiveassistant2023-949a8.firebaseapp.com",
        projectId: "executiveassistant2023-949a8",
        storageBucket: "executiveassistant2023-949a8.appspot.com",
        messagingSenderId: "612446520648",
        appId: "1:612446520648:web:42d1e1176dc364f2f49f7d",
        measurementId: "G-379Z4W3P41"
    });

    const provider = new firebase.auth.OAuthProvider('microsoft.com');
    provider.setCustomParameters({
        tenant: 'common'
    });
    provider.addScope('Mail.Read');
    provider.addScope('Mail.ReadWrite');
    provider.addScope('Mail.ReadBasic');
    provider.addScope('MailboxSettings.ReadWrite');


    //provider.addScope('User.Read');
    provider.addScope('openid');

    firebase.auth().signInWithPopup(provider).then((result) => {
        const credential = result.credential;
        const accessToken = credential.accessToken;
        console.log(`Access token: ${accessToken}`);

        // Use the access token to call the Microsoft Graph API or other Microsoft APIs.
        getUnreadEmails(accessToken);
        //  downloadEmails(accessToken)
    }).catch((error) => {
        console.error(`Failed to authenticate user: ${error}`);
    });
}

async function getUnreadEmails(authtoken) {

    var counter = 0;
    var fromString = "";
    var fromArray = [];
    var subjectString = "";
    var subjectArray = [];
    var bodyString = "";
    var bodyArray = [];
    var emails = null;

    fetch("https://graph.microsoft.com/v1.0/me/mailFolders/inbox/messages?$top=10", {
        headers: {
            Authorization: `Bearer ${authtoken}`
        }
    }
    ).then((response) => {
        console.log("return");
        response.json().then((data) => {
            emails = data.value;
            emails.forEach((email) => {
                console.log(email);
                fromString = fromString + email.from.emailAddress.name + "%split%";
                fromArray.push(email.from.emailAddress.name);
                subjectString = subjectString + email.subject + "%split%";
                subjectArray.push(email.subject);
                bodyString = bodyString + email.bodyPreview + "%split%";
                bodyArray.push(email.bodyPreview);


                //EmailArray.push(email.from.emailAddress.name + "%split%" + email.subject + "%split%" + email.bodyPreview)
                //sleep(5000)
                //tagSingleEmail(email);
                //new Promise(setTimeout(tagSingleEmail, 10000, email))
            });
            console.log(fromString);
           console.log(subjectString);
            console.log(bodyString);
            axios.get("/Home/getTag", {
                params:
                {
                    from: fromString,
                    subject: subjectString,
                    body: bodyString
                }
            })
                .then(res => {
                    console.log(res)
                    const resArray = res.data.split("%spilt%");
                    for (let i = 0; i < resArray.length; i++) {
                        const updateUrl = `https://graph.microsoft.com/v1.0/me/messages/${emails[i].id}`;
                        console.log([resArray[i]]);
                        const payload = {
                            categories: [resArray[i]]
                        };
                        fetch(updateUrl, {
                            method: 'PATCH',
                            headers: {
                                'Content-Type': 'application/json',
                                'Authorization': `Bearer ${authtoken}`

                            },
                            body: JSON.stringify(payload)
                        }).then((response) => {
                            //counter = counter + 1;
                            console.log("added tag");
                            console.log(response);
                        }).catch((error) => {
                            console.error(`Failed to update categories: ${error}`);
                        })
                    };
                });
        });
    });
};
 /*   if ("Notification" in window) {
        // Request permission to show notifications
        Notification.requestPermission().then(function (result) {
            console.log("Notification permission:", result);
            // Show a notification if permission is granted
            if (result === "granted") {
                var notification = new Notification("Executive Assistant", {
                    body: "We have sucessfully tagged " + counter + "emails.",
                  //  icon: "path/to/icon.png",
                });
            }
        });
    }*/


/*async function downloadEmails() {
    const clientID = "cc19483a-abdc-4adc-8fa4-a90d3cade274";
    const scope = "openid profile email Mail.Read Mail.ReadWrite Mail.ReadBasic MailboxSettings.ReadWrite";
    const clientSecret = "UFj8Q~riS1gHu-_LkvwejjNOAEpTEynXZadzDamY";
    const redirectURI = "https://localhost:7150/";
    const authUrl = `https://login.microsoftonline.com/common/oauth2/v2.0/authorize?client_id=${clientID}&response_type=code&redirect_uri=${redirectURI}&scope=openid profile email Mail.Read Mail.ReadWrite Mail.ReadBasic MailboxSettings.ReadWrite`;
    const dialogOptions = { height: 50, width: 50 };

    console.log("preparing..");

    return new Promise((resolve, reject) => {
        Office.context.ui.displayDialogAsync(
            "https://localhost:7150/Home/Temp",
            { height: 50, width: 50 },
            function (result) {
                if (result.status === "succeeded") {
                    console.log("YAYAY")
                    const dialog = result.value;
                    console.log("result.value", result.value);

                        console.log('waiiting');
                        dialog.addEventHandler(
                            Office.EventType.DialogMessageReceived,
                            function (arg) {
                                const message = arg.message;
                                console.log("message here here", message);
                                if (message === "closeDialog") {
                                    debugger
                                    console.log("CLOSING");
                                    dialog.close();
                                    resolve(true);
                                } else {
                                    reject("Unexpected message received from dialog: " + message);
                                }
                            }
                    );

                    console.log("closing the dialog");
                    dialog.close();

                } else {
                    reject("Failed to open dialog: " + result.error.message);
                }
            }
        );
    });*/

/*    return new Promise((resolve, reject) => {
        const dialog = Office.context.ui.displayDialogAsync(redirectURI, dialogOptions);

        return new Promise((resolve, reject) => {
            if (dialog) {
                dialog.addEventHandler(Office.EventType.DialogMessageReceived, async (arg) => {
                    console.log('getting the code');
                    const authCode = getAuthCodeFromURL(arg.message);
                    dialog.close();

                    try {
                        const token = await exchangeAuthCodeForToken(authCode);
                        resolve(token);
                    } catch (err) {
                        reject(err);
                    }
                });
            } else {
                console.log('DEEEPRERESSION')
            }
        });
    });*/
//}


async function authenticate() {
    try {
        const token = await getCode();
        console.log(token);
    } catch (err) {
        console.error(err);
    }
}



async function downloadEmails() {

    console.log("JSJSJSDJDJ");

        //This is web OAuth login

        // Ensure the Firebase SDK has been initialized before continuing.
        firebase.initializeApp({
            apiKey: "AIzaSyDVBPzpvxoABeSMrz_MKhSuO-b6BZm8MgA",
            authDomain: "executiveassistant2023-949a8.firebaseapp.com",
            projectId: "executiveassistant2023-949a8",
            storageBucket: "executiveassistant2023-949a8.appspot.com",
            messagingSenderId: "612446520648",
            appId: "1:612446520648:web:42d1e1176dc364f2f49f7d",
            measurementId: "G-379Z4W3P41"
        });

        const provider = new firebase.auth.OAuthProvider('microsoft.com');
        provider.setCustomParameters({
            tenant: 'common'
        });
        provider.addScope('Mail.Read');
        provider.addScope('Mail.ReadWrite');
        provider.addScope('Mail.ReadBasic');
        provider.addScope('MailboxSettings.ReadWrite');


        //provider.addScope('User.Read');
        provider.addScope('openid');

        firebase.auth().signInWithPopup(provider).then((result) => {
            const credential = result.credential;
            const accessToken = credential.accessToken;
            console.log(`Access token: ${accessToken}`);

            downloadEmailHelper(accessToken);
           // testing(accessToken)

        }).catch((error) => {
            console.error(`Failed to authenticate user: ${error}`);
        });
    


      /* Office.context.ui.displayDialogAsync(
            authUrl,
            { height: 60, width: 40 },
            function (result) {
                if (result.status === Office.AsyncResultStatus.Succeeded) {
                    // Get the authorization code from the dialog URL
                    const authCode = result.value.split('?code=')[1];

                    // Exchange the authorization code for an access token
                    const tokenEndpoint = 'https://login.microsoftonline.com/{tenant}/oauth2/v2.0/token';
                    const requestBody = {
                        client_id: clientID,
                        scope: scope,
                        code: authCode,
                        redirect_uri: redirectURI,
                        grant_type: 'authorization_code',
                        client_secret: clientSecret,
                    };

                    fetch(tokenEndpoint, {
                        method: 'POST',
                        headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
                        body: new URLSearchParams(requestBody),
                    })
                        .then((response) => response.json())
                        .then((data) => {
                            const accessToken = data.access_token;

                            console.log("GOT THE ACCESS TOKEN");

                            // Use the access token to make requests to the Microsoft Graph API
                            // ...
                        })
                        .catch((error) => {
                            console.error(`Failed to get access token: ${error}`);
                        });
                } else {
                    console.error(`Failed to display login page: ${result.error.message}`);
                }
            }
        );*/
}

function getAuthCodeFromURL() {
    debugger
    // Get the current URL
    const url = new URL(window.location.href);

    // Get the auth code from the URL query parameters
    const authCode = url.searchParams.get('code');

    return authCode;
}

async function exchangeAuthCodeForToken(authCode, redirectURI, clientID, clientSecret) {
    const tokenEndpoint = 'https://login.microsoftonline.com/common/oauth2/v2.0/token';

    // Make a POST request to the token endpoint with the auth code and other required parameters
    const response = await fetch(tokenEndpoint, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/x-www-form-urlencoded'
        },
        body: `grant_type=authorization_code&code=${authCode}&redirect_uri=${redirectURI}&client_id=${clientID}&client_secret=${clientSecret}`
    });

    if (!response.ok) {
        throw new Error(`Failed to exchange auth code for token: ${response.statusText}`);
    }

    // Parse the response JSON and extract the access token
    const tokenResponse = await response.json();
    const accessToken = tokenResponse.access_token;

    return accessToken;
}


async function testing(accessToken) {
    console.log("INSIDE TesTING")
    const batchSize = 10;
    let skip = 0;
    let allEmails = [];

    while (true) {
        // Fetch the next batch of emails
        const response = await fetch(`https://graph.microsoft.com/v1.0/me/messages?$select=id,subject,body&$top=${batchSize}&$skip=${skip}`, {
            headers: {
                Authorization: `Bearer ${accessToken}`
            }
        });

        // If the response is not successful, break the loop
        if (!response.ok) {
            console.error(`Failed to fetch emails: ${response.statusText}`);
            break;
        }

        // Parse the JSON response and add the emails to the array
        const data = await response.json();
        allEmails.push(...data.value);

        // If there are no more emails, break the loop
        if (data.value.length < batchSize) {
            break;
        }

        // Update the skip value for the next batch
        skip += batchSize;
    }

    console.log("EMIALS", allEmails)
}

async function downloadEmailHelper(accessToken) {
    //Start downloading the emails

  //  const emailID = new Set();
    // Fetch all emails
    const emailsResponse = await fetch("https://graph.microsoft.com/v1.0/me/mailFolders/inbox/messages?$select=id,subject", {
        headers: {
            Authorization: `Bearer ${accessToken}`,
        },
    });

    const emailsData = await emailsResponse.json();
    const emails = emailsData.value;
    console.log("Line 164 - email data", emails)

    //Used to store the emails that are going to be sent to the backend
    const formData = new FormData();

    // Download each email as MIME content
    for (const email of emails) {
        const id = email.id;

        console.log("subject...", email.subject)

  
        const mimeResponse = await fetch(`https://graph.microsoft.com/v1.0/me/messages/${id}/$value`, {
            headers: {
                Authorization: `Bearer ${accessToken}`,
            },
        });

        //Get the MIME Content and send it to the backend
        const mimeContent = await mimeResponse.blob();
      
      //  formData.append('file', mimeContent, `${email.subject}.eml`);

        // Save MIME content to file in user's document folder
        const filename = `${email.subject}.eml`;
        const downloadLink = document.createElement("a");
        downloadLink.href = URL.createObjectURL(mimeContent);
        downloadLink.download = filename;
       // downloadLink.download = `file:///C:/Users/<username>/Documents/${filename}`;
        downloadLink.click();
    }

/*    axios.post("/Home/sendEmails", formData)
        .then(response => {
            
            console.log(response.data);
        })
        .catch(error => {
            console.error(error);
        });*/
}

/*async function getFrom() {
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
                        //signTags(res.data);
                        console.log("request new tag");
                        const updateUrl = `https://graph.microsoft.com/v1.0/me/messages/${email.id}`;
                        fetch(updateUrl, {
                            method: 'PATCH',
                            headers: {
                                Authorization: `Bearer ${authtoken}`,
                                'Content-Type': 'application/json'
                            },
                            body: JSON.stringify(res.data)
                        }).then((response) => {
                            console.log("added tag");
                            console.log(response);
                        }).catch((error) => {
                            console.error(`Failed to update categories: ${error}`);
                        });
                    });
            }
        })

*/
    /*
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
)*/
//}

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

/*
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
*/