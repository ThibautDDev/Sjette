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

function changeData(i, n) {
    var x = document.getElementsByClassName("lineChartHeaders");
    var htmlCur = x[i-1]
    var htmlNext = x[n-1]
    htmlNext.classList.remove("d-none")
    htmlNext.classList.add("d-flex")

    htmlCur.classList.remove("d-flex")
    htmlCur.classList.add("d-none")
    console.log(i, n)
    console.log(window[`data${n}`])

    myLineChart.data.datasets[0].data = window[`data${n}`]
    myLineChart.update()
}