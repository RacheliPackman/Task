const uri = "/User/Login";
let connected = false;

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
      if (response.status === 200) {
        connected = true;
        response.text();
      }
    })
    .then((result) => {
      if (connected) {
        sessionStorage.setItem("token", result);
        window.location.href = "tasks.html";
      } else alert("שם משתמש או סיסמה שגויים");
    })
    .catch((error) => console.log("error", error));
}
