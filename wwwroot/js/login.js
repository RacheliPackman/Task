const uri = "/User/Login";

function login() {
  const userNameTextbox = document.getElementById("userName").value;
  const passwordTextbox = document.getElementById("password").value;

  var requestOptions = {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({
      id: 0,
      username: userNameTextbox,
      password: passwordTextbox,
      type: "",
    }),
    redirect: "follow",
  };

  fetch(uri, requestOptions)
    .then((response) => {
      if (response.status === 200) return response.text();
      else throw new Error("unauthorize");
    })
    .then((result) => {
      sessionStorage.setItem("token", "Bearer " + result);
      window.location.href = "tasks.html";
    })
    .catch((error) => {
      console.log("error", error);
      alert("error");
    });
}
