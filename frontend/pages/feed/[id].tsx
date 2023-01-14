import axios from 'axios';
import ReactMarkdown from 'react-markdown';
import readingTime from 'reading-time';
import { AUTH_BASE_URL, FeedData, FEED_BASE_URL, UserData } from '../../types';
import { format } from 'date-fns'

interface BlogProps {
    feed: FeedData,
    author: Omit<UserData, "jwt">
}

export default function Blog(props: BlogProps) {

    return (
        <div className="m-4 p-8 grid grid-cols-1 gap-4 lg:w-1/2 mx-auto">
            <div className="flex">
                <div className="avatar">
                    <div className="w-12 rounded-full">
                        <img src="https://placeimg.com/192/192/people" />
                    </div>
                </div>
                <div className="ml-4">
                    <p className="font-semibold">{props.author.firstName} {props.author.lastName}</p>
                    <div>
                        <p className="inline font-thin text-sm align-text-bottom">{format(new Date(props.feed._ts * 1000), 'd MMMM yyyy, HH:mm')}</p>
                        <p className="inline font-bold tracking-widest ml-2">·</p>
                        <p className="inline font-thin text-sm align-text-bottom ml-2">{readingTime(props.feed.content).text}</p>
                    </div>
                </div>
            </div>

            <h1 className="text-4xl font-bold">{props.feed.title}</h1>
            <p className="text-lg">{props.feed.summary}</p>
            <img className="mx-auto w-full" src={props.feed.photo} />
            <div>
                <ReactMarkdown className='leading-relaxed space-y-4' children={props.feed.content} components={{
                    h2({ children }) {
                        return (<div className='space-y-0'><p className='text-2xl font-semibold'>{children}</p><div className="divider"></div></div>);
                    },
                    ul({ children }) {
                        return <ul className="list-disc list-inside indent-4">{children}</ul>
                    },
                    ol({ children }) {
                        return <ol className="list-decimal list-inside indent-4">{children}</ol>
                    },
                    strong({ children }) {
                        return <strong className='font-bold decoration-solid decoration-accent underline'>{children}</strong>
                    },
                    a({ children, href, title }) {
                        return <a className='link link-accent' href={href} title={title}>{children}</a>
                    },
                    em({ children }) {
                        return <em className='text-accent font-italic'>{children}</em>
                    }
                }} />
            </div>
        </div>
    );
}

export async function getServerSideProps(context: { query: { id: string; }; }) {
    const feedReq = await axios.get(`${FEED_BASE_URL}/api/get_feeds`);

    const feeds: FeedData[] = feedReq.data;

    console.log(feeds);
    const feed = feeds.find(x => x.id === context.query.id);

    if (!feed) {
        return {
            redirect: {
                permanent: false,
                destination: '/404'
            }
        }
    }

    const authorReq = await axios.get(`${AUTH_BASE_URL}/api/User/${feed.user}`);

    return {
        props: {
            feed,
            author: authorReq.data
        }
    }
}