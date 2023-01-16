import azure.cosmos.cosmos_client as cosmos_client
import azure.cosmos.exceptions as exceptions
from azure.cosmos.partition_key import PartitionKey


class CosmosDb:

    def __init__(self):

        import os

        settings = {
            'host': os.environ.get('ACCOUNT_HOST', 'https://travel-blog-nst-cosmos.documents.azure.com:443/'),
            'master_key': os.environ.get('CosmosDbAccountKey'),
            'database_id': os.environ.get('COSMOS_DATABASE', 'feeds_db'),
            'container_id': os.environ.get('COSMOS_CONTAINER', 'feeds'),
        }
        self.HOST = settings['host']
        self.MASTER_KEY = settings['master_key']
        self.DATABASE_ID = settings['database_id']
        self.CONTAINER_ID = settings['container_id']

        client = cosmos_client.CosmosClient(self.HOST, {'masterKey': self.MASTER_KEY},
                                            user_agent="CosmosDBPythonQuickstart",
                                            user_agent_overwrite=True)

        try:
            self.db = client.create_database(id=self.DATABASE_ID)
            print('Database with id \'{0}\' created'.format(self.DATABASE_ID))

        except exceptions.CosmosResourceExistsError:
            self.db = client.get_database_client(self.DATABASE_ID)
            print('Database with id \'{0}\' was found'.format(self.DATABASE_ID))

        try:
            self.container = self.db.create_container(id=self.CONTAINER_ID,
                                                      partition_key=PartitionKey(path='/partitionKey'))
            print('Container with id \'{0}\' created'.format(self.CONTAINER_ID))

        except exceptions.CosmosResourceExistsError:

            self.container = self.db.get_container_client(self.CONTAINER_ID)
            print('Container with id \'{0}\' was found'.format(self.CONTAINER_ID))

        print("\nConfiguration done")

    def add_feed(self, feed):

        self.container.create_item(body=feed)
        return "Feed with id {0} added".format(feed["id"])

    def read_feeds(self):

        item_list = list(self.container.read_all_items(max_item_count=100))

        print('Found {0} items'.format(item_list.__len__()))

        for doc in item_list:
            print('Item Id: {0}'.format(doc.get('id')))

        return item_list
