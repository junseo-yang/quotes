# quotes_python_client.py
#   PROG3170 Assignment 
# 
# Revision History
#   Junseo Yang 2023-11-05 Created
#   Junseo Yang 2023-12-10 Updated

import os
import requests
import urllib3
import random
from pathlib import Path

ROOT_DIR = Path(__file__).parent
TEXT_FILE = ROOT_DIR / 'quotes.txt'

# Suppress the warning when http request to localhost 
urllib3.disable_warnings(urllib3.exceptions.InsecureRequestWarning)

# Clear Console
def cls():
    os.system('cls' if os.name=='nt' else 'clear')

def load_quotes():
    # Validate Access Token 
    if access_token == "":
        print()
        print("You should get access token to use the option.")
        return
    
    url = 'https://localhost:7223/api/quotes'

    headers = {
        'Content-Type': 'application/json',
        'Authorization': 'Bearer ' + access_token,
    }

    json_data_list = []
    with open(TEXT_FILE) as f:
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

        print()
        print("Quotes have been loaded successfully.")
    except:
        print()
        print("Something went wrong. Check the API Server.")

def display_random_quote():
    # Validate Access Token 
    if access_token == "":
        print()
        print("You should get access token to use the option.")
        return
    
    url = 'https://localhost:7223/api/quotes'

    headers = {
        'Content-Type': 'application/json',
        'Authorization': 'Bearer ' + access_token,
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
        print()
        print("Something went wrong. Check the API Server.")

def add_new_quote():
    # Validate Access Token
    if access_token == "":
        print()
        print("You should get access token to use the option.")
        return

    quotes_url = 'https://localhost:7223/api/quotes'
    tags_url = 'https://localhost:7223/api/tags'

    headers = {
        'Content-Type': 'application/json',
        'Authorization': 'Bearer ' + access_token,
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
        quote_input = input("Enter a quote: ").strip()
        author_input = input("Enter an author: ").strip()
        tags_input = [tag for tag in input("Enter tags from Available Tags (Use ', ' for separate. e.g., 'life, happiness'): ").strip().split(", ")]
        json = {
            "description": quote_input,
            "author": author_input,
            "tags": tags_input}
        
        # Post to add a new quote
        response = requests.post(quotes_url, json=json, headers=headers, verify=False)
        if (response.ok):
            print()
            print("The quote has been created successfully.")
        else:
            raise Exception('Network Error')
    except:
        print()
        print("Something went wrong. Check the API Server. Or enter valid tags.")

def register_user():
    roles_url = 'https://localhost:7223/api/roles'
    register_url = 'https://localhost:7223/api/register'

    headers = {
        'Content-Type': 'application/json'
    }

    try:
        resp = requests.get(roles_url, headers=headers, verify=False)

        if resp.status_code != 200:
            print()
            print("Error: Check the API first.")
        else:
            print()
            print("Registration")
            first_name = input('Enter a first name: ').strip()
            last_name = input('Enter a last name: ').strip()
            username = input('Enter a username (Required): ').strip()
            password = input('Enter a password (Required): ').strip()
            email = input('Enter an email (Required): ').strip()
            phone_number = input('Enter a phone number: ').strip()

            available_roles = resp.json()["roles"]
            print(f"Available Roles: {available_roles}")
            roles = input('Enter roles (Comma Separated. E.g., QUOTE_USER, QUOTE_MANAGER): ').strip()

            error_message = ""

            # Input Validation
            if (username == ""):
                error_message += "\nError: username is required."
            if (password == ""):
                error_message += "\nError: password is required."
            if (email == ""):
                error_message += "\nError: email is required."
            
            if error_message:
                print(error_message)
            else:
                register_request = {
                    'username': username,
                    'password': password,
                    'email': email
                }

                if (first_name):
                    register_request["firstName"] = first_name
                if (last_name):
                    register_request["lastName"] = last_name
                if (phone_number):
                    register_request["phoneNumber"] = phone_number
                if (roles):
                    register_request["roles"] = roles.split(", ")

                try:
                    resp = requests.post(register_url, headers=headers, json=register_request, verify=False)

                    if resp.status_code == 201:
                        print()
                        print("User registered successfully.")
                    else:
                        results = resp.json()
                        print()
                        print("Errors:")
                        for k, v in results.items():
                            print(f"{k}: {v}")
                except:
                    print()
                    print("Error: Check the API")
    except:
        print()
        print("Error: Check the API or Enter a valid roles.")

def login_user():
    login_url = 'https://localhost:7223/api/login'

    headers = {
        'Content-Type': 'application/json'
    }

    try:
        # User Input
        print()
        print("Login")
        username = input('Enter a username: ').strip()
        password = input('Enter a password: ').strip()
            
        login_request = {
            'userName': username,
            'password': password
        }

        resp = requests.post(login_url, headers=headers, json=login_request, verify=False)
        login_result = resp.json()

        if resp.status_code == 200:
            global access_token 
            access_token = login_result["token"]
            print()
            print("User logged in successfully.")
        else:
            print()
            print("Error: Check your email and password again")
    except:
        print()
        print("Error: Check the API")

# Clear Console
cls()
print("Welcome to QuotesPythonClient!")

quotes_loaded = False

# Store access_token
access_token = ""

while True:
    print()
    print("Options")
    print("1. Register a user")
    print("2. Login")
    print("3. Add a new quote (Login Required)")
    print("4. Display a randomly selected quote (Login Required)")
    if not quotes_loaded:
        print("5. Load quotes to the Web API (One time only) (Login Required)")
    print("6. Exit")
    user_input = input("Select an option by input number only:").strip()

    # Validation
    selection = 0
    try:
        selection = int(user_input)
    except:
        print("Input is not valid. Try Again.")
        continue
    
    # Execute
    if selection == 1:
        register_user()
    elif selection == 2:
        login_user()
    elif selection == 3:
        add_new_quote()
    elif selection == 4:
        display_random_quote()
    elif selection == 5:
        if quotes_loaded:
            print("Quotes has already been loaded.")
        else:
            quotes_loaded = True
            load_quotes()
    elif selection == 6:
        print("Thank you. Bye!")
        break
    else:
        print("Select option 1 ~ 6. Try Again.")