/*//function testing123() {

*//*const clientID = "cc19483a-abdc-4adc-8fa4-a90d3cade274";

// Define the application configuration
const config = {
    auth: {
        clientId: clientID,
        authority: "https://login.microsoftonline.com/common",
    },
    scopes: ["https://graph.microsoft.com/.default"],
};

console.log("created a config");

// Create a PublicClientApplication instance
const pca = new msal.PublicClientApplication(config);

// Attach a click event listener to the Sign In button
const signinButton = document.getElementById("signin");
signinButton.addEventListener("click", () => {
    // Get a device code and verification URL
    pca.acquireTokenByDeviceCode({
        deviceCodeCallback: (response) => {
            console.log(response.message);
            console.log(`Open this URL in a browser: ${response.verificationUri}`);
        },
        scopes: config.scopes,
    }).then((response) => {
        console.log(`Access token: ${response.accessToken}`);

        // Create a Graph client instance with the access token
        const graphClient = MicrosoftGraph.Client.init({
            authProvider: (done) => {
                done(null, response.accessToken);
            },
        });

        // Use the Graph client to get the user's calendar events
        graphClient.api("/me/events").get((error, result) => {
            if (error) {
                console.error(error);
            } else {
                const eventsElement = document.getElementById("events");
                eventsElement.innerText = JSON.stringify(result.value, null, 2);
            }
        });
    }).catch((error) => {
        console.error(error);
    });
});*//*
*//*
    const clientID = "cc19483a-abdc-4adc-8fa4-a90d3cade274";
    const scope = "openid profile email Mail.Read Mail.ReadWrite Mail.ReadBasic MailboxSettings.ReadWrite";
    const clientSecret = "UFj8Q~riS1gHu-_LkvwejjNOAEpTEynXZadzDamY";
    const redirectURI = "https://localhost:7150/";

    const authUrl = `https://login.microsoftonline.com/common/oauth2/v2.0/authorize?client_id=${clientID}&response_type=code&redirect_uri=${redirectURI}&scope=openid profile email Mail.Read Mail.ReadWrite Mail.ReadBasic MailboxSettings.ReadWrite`;
    //window.location.replace(authUrl);
    var myWindow = window.open(authUrl, "_blank");
    var authCode = "123";

    var intervalId = setInterval(function () {
        try {
            //  debugger
            var currentUrl = myWindow.location.href;
            if (currentUrl.indexOf('code=') !== -1) {
                authCode = currentUrl.split('code=')[1];
                clearInterval(intervalId);
                myWindow.close();
                // use authCode as needed

                console.log(authCode);

                const tokenUrl = 'https://login.microsoftonline.com/common/oauth2/v2.0/token?code=${authCode}&grant_type=authorization_code&client_secret=${clientSecret}&client_id=${clientID}&response_type=code&redirect_uri=${redirectURI}&scope=openid profile email Mail.Read Mail.ReadWrite Mail.ReadBasic MailboxSettings.ReadWrite`;

                axios.post(tokenUrl, {
                    grant_type: 'authorization_code',
                    code: authCode,
                    redirect_uri: redirectURI,
                    client_id: clientID,
                    client_secret: clientSecret
                })
                    .then((response) => {
                        console.log("AYYAYAY");
                        const access_token = response.data.access_token;
                        // Use the access token to make requests to the protected resource
                    })
                    .catch((error) => {
                        console.log("NOOOOOO")
                        console.log(error);
                    });

            }
        } catch (err) {
            // debugger
            console.log(err);
            clearInterval(intervalId);
            // myWindow.close();
        }
    }, 1000);*//*


*//*    var myWindow = window.open(authUrl, "_blank");
    
    var currentUrl = myWindow.location.href;
    console.log(currentUrl);*//*

//}

function testing123() {

    const clientID = "cc19483a-abdc-4adc-8fa4-a90d3cade274";
    const scope = "openid profile email Mail.Read Mail.ReadWrite Mail.ReadBasic MailboxSettings.ReadWrite";
    const clientSecret = "UFj8Q~riS1gHu-_LkvwejjNOAEpTEynXZadzDamY";
    const redirectURI = "https://localhost:7150/";

    //  const authUrl = `https://login.microsoftonline.com/common/oauth2/v2.0/authorize?client_id=${clientID}&response_type=code&redirect_uri=${redirectURI}&scope=openid profile email Mail.Read Mail.ReadWrite Mail.ReadBasic MailboxSettings.ReadWrite`;
    //window.location.replace(authUrl);
    /// var myWindow = window.open(authUrl, "_blank");
    var authCode = "123";


    const msalConfig = {
        auth: {
            clientId: clientID,
            authority: 'https://login.microsoftonline.com/common',
            redirectUri: redirectURI,
        }
    };

    const msalInstance = new msal.PublicClientApplication(msalConfig);
    console.log("instance", msalInstance);
    // msalInstance.initialize();

    // Create the main myMSALObj instance
    // configuration parameters are located at authConfig.js
    const myMSALObj = new msal.PublicClientApplication(msalConfig);
    console.log("msal obj", myMSALObj);


    myMSALObj.loginPopup({ scopes: ['openid', 'profile', 'email', 'Mail.Read', 'Mail.ReadWrite', 'Mail.ReadBasic', 'MailboxSettings.ReadWrite'] })
        .then((authResult) => {
            console.log('Access token:', authResult.accessToken);
            console.log('User account:', authResult.account);
        })
        .catch((error) => {
            console.log('Error:', error.message);
        });


    *//*const tokenUrl = 'https://login.microsoftonline.com/common/oauth2/v2.0/token';

    const myInit = {
        method: 'POST',
        mode: 'cors',
        headers: {
            'Content-Type': 'application/x-www-form-urlencoded'
        },
        body: `grant_type=authorization_code&code=${authCode}&redirect_uri=${redirectURI}&client_id=${clientID}&client_secret=${clientSecret}`
    };

    const myRequest = new Request(tokenUrl, myInit);

    fetch(myRequest)
        .then(function (response) {
            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }
            return response.json();
        })
        .then(function (response) {
            console.log(response);
        })
        .catch(function (e) {
            console.log('Error:', e);
        });*//*

    *//*                  axios.post(tokenUrl, {
                          mode: 'cors' // or 'no-cors' or 'same-origin',
                          grant_type: 'authorization_code',
                          code: authCode,
                          redirect_uri: redirectURI,
                          client_id: clientID,
                          client_secret: clientSecret
                      })
                          .then((response) => {
                              console.log("AYYAYAY");
                              const access_token = response.data.access_token;
                              // Use the access token to make requests to the protected resource
                          })
                          .catch((error) => {
                              console.log("NOOOOOO")
                              console.log(error);
                          });*//*

    //  }

}
*/

function testing123() {
    // Create an Auth0 client instance
    var auth0Client = new auth0.WebAuth({
        domain: 'dev-1zacc1eurok2vlyr.us.auth0.com',
        clientID: 'lFKQ8Kl9zz7srzeZy5i9HgyXwEOjVMEP',
    });

    console.log("ready");

    // Authenticate the user and get the tokens
        auth0Client.authorize({
            domain: "dev-1zacc1eurok2vlyr.us.auth0.com",
            clientID: "lFKQ8Kl9zz7srzeZy5i9HgyXwEOjVMEP",
            redirectUri: 'https://dev-1zacc1eurok2vlyr.us.auth0.com/login/callback',
            responseType: 'token id_token',
            scope: "openid profile email Mail.Read Mail.ReadWrite Mail.ReadBasic MailboxSettings.ReadWrite",
            connection: 'ExecutiveAssistant2',
        });

    auth0Client.parseHash((err, authResult) => {
        console.log("authResult", authResult);
        if (authResult && authResult.accessToken && authResult.idToken) {
            // User is authenticated
            console.log(authResult);
        } else if (err) {
            // Handle error
            console.log(err);
        }
    });

    
}