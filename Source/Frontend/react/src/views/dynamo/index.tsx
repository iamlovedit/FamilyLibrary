import { Outlet } from 'react-router-dom';

function DynamoHome() {
    return (
        <div>
            <div>
                Dynamo Home
            </div>
            <Outlet />
        </div>
    );
}

export default DynamoHome;