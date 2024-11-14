export function mapRepositoryOption(data) {
    return data.map((item) => {
        return {
            label: item.name,
            value: item.repoId,
        };
    });
}

export function mapSummaryViewDataChart(data) {
    return data.map((item) => {
        return {
            name: item.name,
            uv: item.uv,
            pv: item.pv,
            amt: item.amt,
        };
    });
}