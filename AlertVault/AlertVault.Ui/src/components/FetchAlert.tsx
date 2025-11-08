import { useEffect, useState } from "react";

export const FetchAlert = () => {
    const [uuid, setUuid] = useState('');

    const fetchUuid = async () => {
        fetch('http://localhost:5038/api/alert', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                interval: "0.00:05:00.0000"
            })
        })
        .then(response => response.json())
        .then(data => {
            setUuid(data.uuid);
        })
        .catch(error => {
            console.error('Error:', error);
        });
    };

    useEffect(() => {
        fetchUuid();
    }, []);

    return (<div className="text-center">
        <p>Your UUID:</p>
        <p>{uuid}</p>
    </div>);
};