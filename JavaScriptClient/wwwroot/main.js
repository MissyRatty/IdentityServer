var config = {

    //where to read the tokens from
    userStore: new Oidc.WebStorageStateStore({ store: window.localStorage }),
    authority: "https://localhost:44338/",
    client_id: "clientJs_id",
    redirect_uri: "https://localhost:44353/Home/SignIn",
    //if it was implicit flow, this would have been: id_token token
    response_type: "code",
    scope: "openid my.OwnDefinedScope ServerApi ClientApi"
};

var userManager = new Oidc.UserManager(config);

var signIn = function () {
    userManager.signinRedirect();
}


userManager.getUser().then(user => {

    console.log("user", user)

    if (user) {
        axios.defaults.headers.common["Authorization"] = "Bearer " + user.access_token;
    }
});

var callServerApi = function () {
    axios.get("https://localhost:44333/data")
        .then(results => {
            console.log("results from API", results);
        }).catch(errors => {
            console.log("errors", errors);
        });
}

//we will use this like a "lock" to determine when to send refresh requests or not./
var refreshing = false;

axios.interceptors.response.use(
    function (response) { return response; },
    function (error) {
        console.log('axios-error: ', error.response);

        //when its 401 (unauthorized), then our access_token may have expired,
        //so let's refresh our token
        var axiosConfig = error.response.config;

        if (error.response.status === 401) {
            console.log('axios-error 401 (Unauthorized)');

            //if already refreshing, then we don't need to make another refresh request
            if (!refreshing) {
                console.log("Start Token Refresh Process......");
                refreshing = true;

                //do the refresh
                return userManager.signinSilent().then(user => {

                    console.log("response from silent signin (aka): new user", user);

                    //Update the axios Http client and request
                    axios.defaults.headers.common["Authorization"] = "Bearer " + user.access_token; // set the header for your axios http client
                    axiosConfig.headers["Authorization"] = "Bearer " + user.access_token;   //reset http request config from error.response to use the new access_token from the siginSilent

                    //retry the http request

                    console.log("new config for our retry", axiosConfig);
                    return axios(axiosConfig);
                });
            }

        }

        return Promise.reject(error);
    }
);