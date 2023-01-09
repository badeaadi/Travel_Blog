import BlogCard from "../../components/BlogCard";

const mockedBlogs = [
    {
        title: "Lorem ipsum",
        summary: "Lorem ipsum 2",
        id: '1',
        photo: "https://picsum.photos"
    },
    {
        title: "Lorem ipsum 414114",
        summary: "Lorem ipsum 3",
        id: '2',
        photo: "https://picsum.photos"
    },
    {
        title: "Lorem ipsum 22",
        summary: "Lorem ipsum 4",
        id: '3',
        photo: "https://picsum.photos"
    },
    {
        title: "Lorem ipsum 2231311",
        summary: "Lorem ipsum 433333",
        id: '4',
        photo: "https://picsum.photos"
    }
];

export default function Feed() {

    return (
        <div className="m-4 p-4 grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
            {mockedBlogs.map(blog => {
                return <BlogCard title={blog.title} summary={blog.summary} id={blog.id} photo={blog.photo} key={blog.id} />
            })}
        </div>
    );
}