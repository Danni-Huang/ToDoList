import requests
from datetime import datetime, timedelta
import json


headers = {
    'Content-Type': 'application/json'
}

# Note: your port number might be different
url = 'https://localhost:7187/tasks'


task_categories = ['Home', 'Work', 'Shopping']

task_desc = input('what is the description of your task?')
task_category_index = int(input('what is the task category: 1. Home, 2. Work, 3. Shopping: '))
num_days_due = int(input('How many days is the task due: '))

due_date = datetime.now() + timedelta(days=num_days_due)

new_task = {
    'description': task_desc,
    'category': task_categories[task_category_index - 1],
    'duedate': due_date.isoformat()
}

# use requests package to issue a POST:
resp = requests.post(url, json=new_task, verify=False)

# try to access URL in the location header:
if 'Location' in resp.headers:
    print(f'new task is at: {resp.headers["Location"]}')
else:
    print('there was a problem adding the task.')
