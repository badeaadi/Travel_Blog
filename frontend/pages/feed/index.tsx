import BlogCard from "../../components/BlogCard";
import {FiPlus} from "react-icons/fi";
import { useRouter } from "next/router";
import axios from "axios";
import { FEED_BASE_URL, FeedData } from "../../types";

interface FeedProps {
    feed: FeedData[]
}

export default function Feed(props: FeedProps) {
    const router = useRouter();

    return (
        <>
            <div className="m-4 p-4 grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
                {props.feed.map(blog => {
                    return <BlogCard title={blog.title} summary={blog.summary} id={blog.id} photo={blog.photo} key={blog.id} />
                })}
            </div>
            <button onClick={() => router.push('/feed/create')} className="btn btn-circle btn-primary text-xl text-primary-content fixed bottom-6 left-6"><FiPlus /></button>
        </>
    );
}

export async function getServerSideProps() {
    const feedReq = await axios.get(`${FEED_BASE_URL}/api/get_feeds`);

    return {
        props: {
            feed: feedReq.data
        }
    }
}