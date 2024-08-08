import { useLocation, useSearchParams } from 'react-router-dom';

function Gitee() {
    const location = useLocation();
    const [searchParams, setSearchParams] = useSearchParams();
    const code = searchParams.get('code')

    return (
        <>
            {code}
        </>
    )
}

export default Gitee