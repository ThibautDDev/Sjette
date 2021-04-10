# -*- coding: utf-8 -*-
"""
Created on Sat Apr 10 16:52:39 2021

@author: ThibautD
"""
import random


def randomId(Num=15):
    return random.randint(1, Num)

def randomActivityType():
    return random.choice(["Running", "Cycling", "Hiking"])

def randomActivityName(ActivityType, i):
    return f"{ActivityType}{i}"

def randomCalories(ActivityType, Distance, Time, BodyMass=70):
    #https://www.coachmag.co.uk/running/8065/how-many-calories-does-running-burn#:~:text=Broadly%20speaking%2C%20the%20estimate%20of,80%2D100%20calories%20per%20kilometre.
    #https://captaincalculator.com/health/calorie/calories-burned-cycling-calculator/
    #https://metscalculator.com/
    if(ActivityType=="Running"): 
        MET = 10
    elif(ActivityType=="Cycling"): 
        MET = 12
    else:
        MET = 6
    return int(((MET * BodyMass * 3.5) / 200)*Time)

def randomDistance(ActivityType):
    if(ActivityType=="Running"): 
        i=1
        j=60
    elif(ActivityType=="Cycling"): 
        i=5
        j=200
    else:
        i=2
        j=150
    return round(random.uniform(i, j), 2)

def randomTime(ActivityType, Distance):
    if(ActivityType=="Running"): 
        Ave = random.uniform(6, 15)
    elif(ActivityType=="Cycling"): 
        Ave = random.uniform(12, 40)
    else:
        Ave = random.uniform(3, 5)
    return Distance/(Ave/60)

def convertTime(time):
    hours = int(time/60)
    minutes = int(time-(hours*60))
    seconds = int((time-(hours*60)-minutes)*60)
    if hours < 10:
        hours = f"0{hours}"
    if minutes < 10:
        minutes = f"0{minutes}"
    if seconds < 10:
        seconds = f"0{seconds}"
    return f"{hours}:{minutes}:{seconds}"

def randomStartTime(maxMonth=12):
    day = random.randint(1, 28)
    if day < 10:
        day = f"0{day}"
    month = random.randint(1, maxMonth)
    if month < 10:
        month = f"0{month}"
    time = random.randint(10, 15)
    
    return f"2021-{month}-{day} {time}:00:00"
        
def randomGear():
     return random.choice(["Nike Shoes", "Asics Shoes", "Polar Watch", "AppleWatch", "NULL"])



def main():
    Amount = int(input("Hoeveel datalijnen wilt u genereren? \n> "))
    maxMonth = int(input("Tot en met welk maandgetal? \n> "))
    f = open("dataFromPython.txt", "w+")
    

    for i in range(1, 1+Amount):
        Id = randomId()
        Type = randomActivityType()
        Name = randomActivityName(Type, i)
        Dist = randomDistance(Type)
        TimeInMin = randomTime(Type, Dist)
        
        while TimeInMin > 1440:                 #REDO BECAUSE OF MORE THAN 24H
            Type = randomActivityType()
            Name = randomActivityName(Type, i)
            Dist = randomDistance(Type)
            TimeInMin = randomTime(Type, Dist)
            
        Time = convertTime(TimeInMin)
        Cal = randomCalories(Type, Dist, TimeInMin)
        StartT = randomStartTime(maxMonth)
        Gear = randomGear()
        line = f"({Id}, '{Type}', '{Name}', {Cal}, {Dist}, '{Time}', '{StartT}', '{Gear}')"
        if(i < Amount):
            line += ",\n"
        else:
            line += ";"
        f.write(line)
    f.close()
main()