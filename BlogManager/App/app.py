import flask
from dotenv import load_dotenv
from flask import request
from waitress import serve

from cosmos_db import CosmosDb
from util import Util

app = flask.Flask(__name__)

chosen_host = '0.0.0.0'
chosen_port = 5500


@app.route('/api/health', methods=['GET'])
def health():
    return "Blog Manager healthy"


@app.route('/api/create_feed', methods=['POST'])
def create_feed():
    try:
        feed = util.verify_create_feed_valid(request=request.get_json())
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
    util = Util()

    print("Starting Python on http://{}:{}".format(chosen_host, chosen_port))
    serve(app, host=chosen_host, port=chosen_port)
