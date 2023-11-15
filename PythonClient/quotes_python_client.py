import os
import requests
import urllib3
import random

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

def display_random_quote():
    url = 'https://localhost:7223/api/quotes'

    headers = {
        'Content-Type': 'application/json'
    }

    try:
        # Send a get request to get all quotes
        response = requests.get(url, headers=headers, verify=False).json()
        quotes = response['quotes']

        # Select a random quote
        quote = random.choice(quotes)

        # Print a random quote
        print()
        print("A random quote")
        print(f"'{quote['description']}' by '{quote['author']}' (Likes: {quote['like']})")
    except:
        print("Something went wrong. Check the API Server.")

def add_new_quote():
    quotes_url = 'https://localhost:7223/api/quotes'
    tags_url = 'https://localhost:7223/api/tags'

    headers = {
        'Content-Type': 'application/json'
    }

    try:
        # Get Available Tags
        print()
        print("Available Tags")
        response = requests.get(tags_url, headers=headers, verify=False).json()
        tags = [tag['name'] for tag in response['tags']]
        print(tags)

        # Get User Input to create quote
        print()
        quote_input = input("Enter a quote: ")
        author_input = input("Enter an author: ")
        tags_input = [tag for tag in input("Enter tags from Available Tags (Use ', ' for separate. e.g., 'life, happiness'): ").split(", ")]
        json = {
            "description": quote_input,
            "author": author_input,
            "tags": tags_input}
        
        # Post
        response = requests.post(quotes_url, json=json, headers=headers, verify=False)
        if (response.ok):
            print("The quote has been created successfully.")
        else:
            raise Exception('Network Error')
    except:
        print("Something went wrong. Check the API Server. Or enter valid tags.")

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
        add_new_quote()
    elif selection == 2:
        display_random_quote()
    elif selection == 3:
        if quotes_loaded:
            print("Quotes has already been loaded.")
        else:
            quotes_loaded = True
            load_quotes()
    else:
        print("Select option 1 ~ 3. Try Again.")