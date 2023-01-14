def _verify_create_feed_valid(request: dict) -> (str, str):
    link = request["link"]
    markdown = request["markdown"]
    # Also, we will verify token here

    return link, markdown


def _verify_get_feeds_valid(request: dict) -> (str, str):
    pass
    # we will verify token here

