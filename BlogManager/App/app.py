import flask
from dotenv import load_dotenv
from waitress import serve

app = flask.Flask(__name__)

chosen_host = '0.0.0.0'
chosen_port = 5500

@app.route('/api/health')
def health():
    return "Healthy"


if __name__ == "__main__":
    load_dotenv(override=False)
    print("Starting Python on http://{}:{}".format(chosen_host, chosen_port))
    serve(app, host=chosen_host, port=chosen_port);