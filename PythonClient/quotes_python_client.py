import os
import requests
import json
import urllib3

# Suppress the warning when http request to localhost 
urllib3.disable_warnings(urllib3.exceptions.InsecureRequestWarning)

def cls():
    os.system('cls' if os.name=='nt' else 'clear')

def load_quotes():
    url = 'https://localhost:7223/api/quotes'

    headers = {
        'Content-Type': 'application/json'
    }

    json_data_list = []
    with open('quotes.txt') as f:
        lines = f.readlines() # list containing lines of file

        for line in lines:
            line = line.strip() # remove leading/trailing white spaces
            data = [item.strip() for item in line.split(' ~')]
            json = {}
            json["description"] = data[0]
            json["author"] = data[1]
            json["tags"] = [item for item in data[2].split(', ')]

            json_data_list.append(json)

    try:
        for json in json_data_list:
            requests.post(url, json=json, headers=headers, verify=False)
        
        print("Quotes have been loaded successfully.")
    except:
        print("Something went wrong. Check the API Server.")

# Clear Console
cls()
print("Welcome to QuotesPythonClient!")

quotes_loaded = False

while True:
    print()
    print("Options")
    print("1. Add a new quote")
    print("2. Display a randomly selected quote")
    if not quotes_loaded:
        print("3. Load quotes to the Web API (One time only)")
    user_input = input("Select an option by input number only:")

    # Validation
    selection = 0
    try:
        selection = int(user_input)
    except:
        print("Input is not valid. Try Again.")
        continue
    
    # Execute
    if selection == 1:
        print(selection)
    elif selection == 2:
        print(selection)
    elif selection == 3:
        if quotes_loaded:
            print("Quotes has already been loaded.")
        else:
            quotes_loaded = True
            load_quotes()
    else:
        print("Select option 1 ~ 3. Try Again.")


def write_tasks_to_file(task_data):
    for t in task_data:
        # opening a file for writing named by the task's ID:
        curr_task_id = t['taskId']
        task_file = open(f'data/{curr_task_id}_.json', 'w')

        # write/dump to the file the JSON for that task:
        task_file.write(json.dumps(t, indent=4))

        # close the file:
        task_file.close()

# call our 2 functions:
tasks = get_all_tasks()
write_tasks_to_file(tasks)

