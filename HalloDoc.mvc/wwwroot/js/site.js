

function myFunction() {
    var e1 = document.body;
    e1.classList.toggle("dark-mode");

    const e2 = document.getElementById('navbar');
    e2.classList.toggle('dark-mode');

    const e3 = document.getElementById('footr');
    e3.classList.toggle('dark-mode');

    const e4 = document.getElementById('main-cont');
    e4.classList.toggle('dark-mode');




    if (e1.classList.contains('dark-mode')) {
        e2.style.backgroundColor = 'black';
        e3.style.backgroundColor = 'black';
        e4.style.backgroundColor = 'rgba(255, 255, 255, 0.884)';
        e4.style.color = 'black';


    } else {
        e2.style.backgroundColor = 'white';
        e3.style.backgroundColor = '';
        e4.style.backgroundColor = '';
        e4.style.color = ''

    }

    var img = e1.querySelector(".light")
    img.src.includes("dark") ? img.src = "../images/light.png" : img.src = "../images/dark.png";



}


