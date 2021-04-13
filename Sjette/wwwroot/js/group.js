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
    var htmlCur = x[i + step]
    var htmlNext = x[n + step]

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
    myLineChart = dictOfCharts[step / 6]
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

    changeInnerHtml(groupName)
    //console.log()
}

function changeInnerHtml(groupName) {
    //LEAVE GROUP MODAL
    var x = document.getElementById("leaveGroupText")
    x.innerHTML = `Are you sure you want to leave '${groupName}'?`
    var x = document.getElementById("leaveGroupName")
    x.value = groupName

    //DELETE GROUP MODAL
    var x = document.getElementById("deleteGroupText")
    x.innerHTML = `Are you sure you want to delete '${groupName}'?`
    var x = document.getElementById("deleteGroupName")
    x.value = groupName

    //ADD MEMBER TO GROUP MODAL
    var x = document.getElementById("addMemberGroupName")
    x.value = groupName

    //REMOVE MEMBER TO GROUP MODAL
    var x = document.getElementById("removeMemberGroupName")
    x.value = groupName
    var x = document.getElementById("UserToRemove")
    x.innerHTML = ""; //REMOVE CURRENT OPTIONS
    for (var lst in groupDictionairy[groupName]) {
        var innerOption = document.createElement("option");
        var obj = groupDictionairy[groupName][lst];
        if (obj["pk_UserID"] != userId){
            var name = `${obj["firstName"]} ${obj["lastName"]}`;
            innerOption.value = obj["pk_UserID"];
            innerOption.innerHTML = name;
            x.appendChild(innerOption);
        }
    }

    //ADMIN TRANSFER GROUP MODAL
    var x = document.getElementById("transferAdminGroupName")
    x.value = groupName
    var x = document.getElementById("UserToTransfer")
    x.innerHTML = ""; //REMOVE CURRENT OPTIONS
    for (var lst in groupDictionairy[groupName]) {
        var innerOption = document.createElement("option");
        var obj = groupDictionairy[groupName][lst];
        if (obj["pk_UserID"] != userId){
            var name = `${obj["firstName"]} ${obj["lastName"]}`;
            innerOption.value = obj["pk_UserID"];
            innerOption.innerHTML = name;
            x.appendChild(innerOption);
        }
    }
}