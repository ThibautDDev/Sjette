function changeGroupCard(i, n) {
    // This function will display a certain card with information on press.
    var x = document.getElementsByClassName("groupCards");    //Get all elements with fitnessCards as className
    var htmlCur = x[i]
    var htmlNext = x[n]

    // Change displaysettings between current and next card
    htmlNext.classList.remove("d-none")
    htmlCur.classList.add("d-none")
}


function changeMutualFriendsCard(i, n) {
    // This function will display a certain card with information on press.
    var x = document.getElementsByClassName("mutualFriendsCard");    //Get all elements with fitnessCards as className
    var htmlCur = x[i]
    var htmlNext = x[n]

    // Change displaysettings between current and next card
    htmlNext.classList.remove("d-none")
    htmlCur.classList.add("d-none")
}