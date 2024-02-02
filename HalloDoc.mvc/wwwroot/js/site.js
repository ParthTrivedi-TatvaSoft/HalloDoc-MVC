// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.



function myFunction() {
    var element = document.body;
    element.classList.toggle("dark-mode");
    var img = element.querySelector(".light")
    img.src.includes("dark") ? img.src = "images/light.png" : img.src = "images/dark.png";
}
var modal = document.getElementById("myModal");


var btn = document.getElementById("myBtn");

$(document).ready(function () {
    modal.style.display = "block";
});

var span = document.getElementsByClassName("close")[0];

// When the user clicks the button, open the modal 
btn.onclick = function () {
    modal.style.display = "block";
}

// When the user clicks on <span> (x), close the modal
span.onclick = function () {
    modal.style.display = "none";
}

// When the user clicks anywhere outside of the modal, close it
window.onclick = function (event) {
    if (event.target == modal) {
        modal.style.display = "none";
    }
}
