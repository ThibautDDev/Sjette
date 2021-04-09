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


function changeData(i, n, step, groupName, activity) {
    // This function will display a certain chart with information on press.
    var x = document.getElementsByClassName("lineChartHeaders");    //Get all headers with lineChartHeaders as className
    var htmlCur = x[i+step]
    var htmlNext = x[n+step]

    // Change displaysettings between current and next card
    htmlNext.classList.remove("d-none")
    htmlNext.classList.add("d-flex")

    htmlCur.classList.remove("d-flex")
    htmlCur.classList.add("d-none")
    //console.log(x)
    //console.log(i, n)
    //console.log(window[`data${name}`])

    // Update chart data with smooth transition and dynamic data based on the n
    //console.log(step/6, activity)
    //console.log(dictOfCharts[step/6])
    myLineChart = dictOfCharts[step/6]
    //console.log(LineChartData[groupName][activity])

    myLineChart.data.datasets[0].data = LineChartData[groupName][activity]
    if (activity == "Calories") myLineChart.data.datasets[0].label = "Calories"
    if (activity == "Distance") myLineChart.data.datasets[0].label = "Distance"
    myLineChart.update()
    //console.log()
}


//RESETTING DATA WITH RIGHT HEADER
function updateData(el, step, groupName, activity) {
    var x = document.getElementsByClassName("lineChartHeaders");
    for (i = 0; i < x.length; i++) {
        x[i].classList.remove("d-none")
        x[i].classList.remove("d-flex")
        x[i].classList.add("d-none")
    }

    var htmlCur = x[step]
    htmlCur.classList.remove("d-none")
    htmlCur.classList.add("d-flex")

    //console.log(step/6, activity)
    //console.log(dictOfCharts[step/6])
    myLineChart = dictOfCharts[step / 6]
    //console.log(LineChartData[groupName][activity])

    myLineChart.data.datasets[0].data = LineChartData[groupName][activity]
    myLineChart.data.datasets[0].label = "Activities"
    myLineChart.update()
    //console.log()
}