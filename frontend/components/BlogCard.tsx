import Link from "next/link";

interface BlogCardProps {
    title: string;
    summary: string;
    id: string;
    photo: string;
}

export default function BlogCard(props: BlogCardProps) {
    return (
        <>
            <div className="card w-full bg-base-200 shadow-xl max-w-md">
                <figure><img src={`${props.photo}`} /></figure>
                <div className="card-body">
                    <h2 className="card-title">{props.title}</h2>
                    <p>{props.summary}</p>
                    <div className="card-actions justify-end">
                        <Link href={`/feed/${props.id}`}>
                            <button className="btn btn-primary">View</button>
                        </Link>
                    </div>
                </div>
            </div>
        </>
    );
}