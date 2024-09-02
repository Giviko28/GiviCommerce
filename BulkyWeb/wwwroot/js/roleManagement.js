let selectRole = document.querySelector("#roleSelect");
let selectCompany = document.querySelector("#companySelect");

selectRole.addEventListener("change", () => {
    if (selectRole.options[selectRole.selectedIndex].text == "Company") {
        selectCompany.style.display = "block";
    }
    else {
        selectCompany.style.display = "none";
    }
})