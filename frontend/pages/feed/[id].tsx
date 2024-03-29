import axios from 'axios';
import ReactMarkdown from 'react-markdown';
import readingTime from 'reading-time';
import { AUTH_BASE_URL, FeedData, FEED_BASE_URL, UserData } from '../../types';
import { format } from 'date-fns'
import { useRouter } from 'next/router';

interface BlogProps {
    feed: FeedData,
    author: Omit<UserData, "jwt">
}

export default function Blog(props: BlogProps) {
    const router = useRouter();

    return (
        <div className="m-4 p-8 grid grid-cols-1 gap-4 lg:w-1/2 mx-auto">
            <div className="flex">
                <div className="avatar hover:cursor-pointer" onClick={() => router.push(`/profile/${props.feed.user}`)}>
                    <div className="w-12 rounded-full">
                        <img src={props.author.thumbnailUrl ?? 'data:image/png;base64,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'} />
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