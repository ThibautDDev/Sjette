function changeFitnessCard(i, n) {
    var x = document.getElementsByClassName("fitnessCards");
    var htmlCur = x[i]
    var htmlNext = x[n]

    htmlNext.classList.remove("d-none")
    htmlCur.classList.add("d-none")
}

function changeRankingCard(i, n) {
    var x = document.getElementsByClassName("rankingCards");
    var htmlCur = x[i]
    var htmlNext = x[n]

    htmlNext.classList.remove("d-none")
    htmlCur.classList.add("d-none")
}