import flask
from dotenv import load_dotenv
from waitress import serve

import uuid

from service import CosmosDb
from util import _verify_create_feed_valid, _verify_get_feeds_valid
from flask import request

app = flask.Flask(__name__)

chosen_host = '0.0.0.0'
chosen_port = 5500


@app.route('/api/health', methods=['GET'])
def health():
    return "Blog Manager healthy"


@app.route('/api/create_feed', methods=['POST'])
def create_feed():
    try:
        link, markdown = _verify_create_feed_valid(request=request.get_json())
        feed = {
            'link': link,
            'markdown': markdown,
            'id': str(uuid.uuid4())
        }
        return cosmosDb.add_feed(feed)

    except Exception as e:
        if hasattr(e, "code") and e.code is not None:
            code = e.code
        elif hasattr(e, "http_status") and e.http_status is not None:
            code = e.http_status
        msg = str(e)
        flask.abort(code, msg)


@app.route('/api/get_feeds', methods=['GET'])
def get_feeds():
    try:
        _verify_get_feeds_valid(request=request)
        return cosmosDb.read_feeds()
    except Exception as e:
        if hasattr(e, "code") and e.code is not None:
            code = e.code
        elif hasattr(e, "http_status") and e.http_status is not None:
            code = e.http_status
        msg = str(e)
        flask.abort(code, msg)


if __name__ == "__main__":
    load_dotenv(dotenv_path='./.env', override=False)
    cosmosDb = CosmosDb()

    print("Starting Python on http://{}:{}".format(chosen_host, chosen_port))
    serve(app, host=chosen_host, port=chosen_port)
