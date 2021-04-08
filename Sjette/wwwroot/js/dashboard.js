function changeFitnessCard(i, n) {
    // This function will display a certain card with information on press.
    var x = document.getElementsByClassName("fitnessCards");    //Get all elements with fitnessCards as className
    var htmlCur = x[i]
    var htmlNext = x[n]

    // Change displaysettings between current and next card
    htmlNext.classList.remove("d-none")
    htmlCur.classList.add("d-none")
}

function changeRankingCard(i, n) {
    // This function will display a certain card with information on press.
    var x = document.getElementsByClassName("rankingCards");    //Get all elements with rankingCards as className
    var htmlCur = x[i]
    var htmlNext = x[n]

    // Change displaysettings between current and next card
    htmlNext.classList.remove("d-none")
    htmlCur.classList.add("d-none")
}

function changeData(i, n) {
    // This function will display a certain chart with information on press.
    var x = document.getElementsByClassName("lineChartHeaders");    //Get all headers with lineChartHeaders as className
    var htmlCur = x[i-1]
    var htmlNext = x[n - 1]

    // Change displaysettings between current and next card
    htmlNext.classList.remove("d-none")
    htmlNext.classList.add("d-flex")

    htmlCur.classList.remove("d-flex")
    htmlCur.classList.add("d-none")
    //console.log(i, n)
    //console.log(window[`data${n}`])

    // Update chart data with smooth transition and dynamic data based on the n
    myLineChart.data.datasets[0].data = window[`data${n}`]
    myLineChart.update()
}