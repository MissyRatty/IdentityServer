var config = {
    authority: "https://localhost:44338/",
    client_id: "clientJs_id",
    redirect_uri: "https://localhost:44353/Home/SignIn",
    response_type: "id_token token",
    scope: "openid ServerApi"
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