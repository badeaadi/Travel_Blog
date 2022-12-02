import { Button } from "flowbite-react";
import { useState } from "react";

export default function ExampleComponent() {
    const [counter, setCounter] = useState(0);
    
    return <Button onClick={() => setCounter(counter + 1)}>Count: {counter}</Button>
}