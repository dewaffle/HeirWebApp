function changeHomeContent() {
    var img = document.getElementById("homeImage");
    var text = document.getElementById("dynamicText");

    // Check if the current image source contains 'family.jpg'
    if (img.src.match("family.jpg")) {
        // Revert to original state
        img.src = "/images/heir.jpg";       
        text.innerHTML = "Original project message";
        text.style.color = "black";
    }
    else {
        // Change to the new state
        img.src = "/images/family.jpg";     
        text.innerHTML = "The image and text were changed using JavaScript.";
        text.style.color = "darkgreen";
    }
}

function mouseOverMessage()
{
    document.getElementById("hoverMessage").innerHTML =
    "Mouse is over the image";
}


function mouseOutMessage()
{
    document.getElementById("hoverMessage").innerHTML =
    "Mouse left the image";
}


function validateLogin()
{
    var email = document.getElementById("loginEmail").value;

    var password = document.getElementById("loginPassword").value;

    if(email == "")
    {
        alert("Please enter your email");
        return false;
    }

    if(password == "")
    {
        alert("Please enter your password");
        return false;
    }

    alert("Login Successful");
    return true;
}


function validateSignup()
{
    var fullname = document.getElementById("fullname").value;

    var email = document.getElementById("signupEmail").value;

    if(fullname == "")
    {
        alert("Please enter full name");
        return false;
    }

    if(email == "")
    {
        alert("Please enter email");
        return false;
    }

    alert("Account Created");
    return true;
}


function focusFunction(element)
{
    element.style.backgroundColor = "lightyellow";
}


function blurFunction(element)
{
    element.style.backgroundColor = "white";
}


function createList()
{
    var text = "";
    var i;

    text = "<ul>";

    for(i = 1; i <= 5; i++)
    {
        text += "<li>Relationship Level " + i + "</li>";
    }

    text += "</ul>";

    document.getElementById("dynamicArea").innerHTML = text;
}