import json
import uuid

import jwt
import requests


class Util:

    def __init__(self):
        self.identity_url = 'http://traver-blog-nst-srv-dns.northeurope.cloudapp.azure.com/identity/'

    def verify_create_feed_valid(self, request: dict):
        token = request["token"]
        user = self._verify_token(token)

        feed = {
            'id': str(uuid.uuid4()),
            'title': request["title"],
            'photo': request["photo"],
            'summary': request["summary"],
            'content': request["content"],
            'user': user
        }
        return feed

    def _verify_token(self, token: str):
        url = self.identity_url + 'api/Token/validate'
        data = {'token': token}
        response = requests.post(url, data=json.dumps(data),
                                 headers={'Content-type': 'application/json'})
        if not response.ok:
            raise Exception

        decoded = jwt.decode(token, options={"verify_signature": False}, algorithms=["HS256"])
        return decoded['sub']
