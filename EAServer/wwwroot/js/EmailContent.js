/*Office.onReady(() => { 
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
        prompt: 'consent',
        tenant: 'common'
    });

    firebase.auth().signInWithPopup(provider).then((result) => {
        const credential = result.credential;
        const accessToken = credential.accessToken;
        console.log(`Access token: ${accessToken}`);
        // Use the access token to call the Microsoft Graph API or other Microsoft APIs.
    }).catch((error) => {
        console.error(`Failed to authenticate user: ${error}`);
    });
});






*/