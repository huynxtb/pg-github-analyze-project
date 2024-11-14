import React, { useEffect, useState } from 'react';
import { Button, DatePicker, Select, Flex } from 'antd';
import { useTheme } from '@mui/material/styles';
import {
  Grid,
  Card,
  CardHeader,
  CardContent,
  Typography,
  Divider
} from '@mui/material';
import {
  LineChart,
  BarChart,
  Bar,
  Line,
  XAxis,
  YAxis,
  CartesianGrid,
  Tooltip,
  Legend,
  Brush,
  AreaChart,
  Area,
  ResponsiveContainer,
} from 'recharts';
import ReportCard from './ReportCard';
import { gridSpacing } from 'config.js';
import {
  mapRepositoryOption
} from 'utils';

const { RangePicker } = DatePicker;

const ViewCloneTooltip = ({ active, payload, label }) => {
  if (active && payload && payload.length) {
    console.log('payload:', payload);
    return (
      <div className="custom-tooltip">
        <p className="label">{`${label}`}</p>
        <p className="intro">Views: {`${payload[0].value}`}</p>
        <p className="intro">Uniques: {`${payload[0].payload.uniques}`}</p>
      </div>
    );
  }
  return null;
};

const CustomizedLabel = ({ x, y, stroke, value }) => {
  return (
    <text x={x} y={y} dy={-4} fill={stroke} fontSize={10} textAnchor="middle">
      {value}
    </text>
  );
}

const CustomizedAxisTick = ({ x, y, stroke, payload }) => {
  return (
    <g transform={`translate(${x},${y})`}>
      <text x={0} y={0} dy={16} textAnchor="end" fill="#666" transform="rotate(-35)">
        {payload.value}
      </text>
    </g>
  );
}

const Default = () => {
  const BASE_ADDRESS = import.meta.env.VITE_APP_API_URL;
  const GRID_COL = 4;
  const theme = useTheme();
  const [summaryData, setSummaryData] = useState(null);
  const [repositoryOpt, setRepositoryOpt] = useState([]);
  const [filterd, setFiltered] = useState(false);
  const [dataFilter, setDataFilter] = useState({});
  const [selectedRepository, setSelectedRepository] = useState(null);
  const [selectedDateRange, setSelectedDateRange] = useState(null);
  const [repoFilterd, setRepoFiltered] = useState(null);
  const [lastSync, setLastSync] = useState(null);
  const [loading, setLoading] = useState(false);

  console.log('BASE_ADDRESS:', BASE_ADDRESS);
  
  const fetchSummaryData = async () => {
    setLoading(true);
    try {
      let requestParams = '';
      if (Object.keys(dataFilter).length > 0) {
        requestParams = `?repoId=${dataFilter.repoId}&startDate=${dataFilter.startDate}&endDate=${dataFilter.endDate}`;
      }
      const response = await fetch(`${BASE_ADDRESS}/traffic/data/summary${requestParams}`);
      if (!response.ok) {
        throw new Error('Network response was not ok');
      }
      const result = await response.json();
      setSummaryData(result);
      setRepositoryOpt(mapRepositoryOption(result.data.repository?.repositories));
      setRepoFiltered(result.data.filterData);
      setLoading(false);
    } catch (error) {
      console.error('There was a problem with your fetch operation:', error);
      setLoading(false);
    } finally {
      setLoading(false);
    }
  };

  const fetchHistorySync = async () => {
    try {
      const response1 = await fetch(`${BASE_ADDRESS}/traffic/sync/history/last-sync`);
      if (!response1.ok) {
        throw new Error('Network response was not ok');
      }
      const result1 = await response1.json();
      setLastSync(result1.data);
    } catch (error) {
      console.error('There was a problem with your fetch operation:', error);
    }
  };

  useEffect(() => {
    fetchSummaryData();
    fetchHistorySync();
  }, [filterd]);

  const handleRepositoryChange = (value) => {
    console.log('Repository changed:', value);
    setSelectedRepository(value);
    setDataFilter((prevData) => ({
      ...prevData,
      repoId: value
    }));
  }

  const handleDateChange = (date, dateString) => {
    console.log('Date changed:', dateString);
    setSelectedDateRange(date);
    setDataFilter((prevData) => ({
      ...prevData,
      startDate: dateString[0],
      endDate: dateString[1]
    }));
  }

  const handleGenerateClick = () => {
    setFiltered(!filterd);
  }

  const handleClearClick = () => {
    setSelectedRepository(null);
    setSelectedDateRange(null);
    setDataFilter({});
    setFiltered(!filterd);
  }

  return (
    <Grid container spacing={gridSpacing}>

      <Grid item xs={12}>
        <Grid container spacing={gridSpacing}>
          <Grid item lg={GRID_COL} sm={6} xs={12}>
            <ReportCard
              primary={summaryData?.data?.repository?.total?.toString()}
              secondary="Total Repositories"
              color={theme.palette.warning.main}
            />
          </Grid>
          <Grid item lg={GRID_COL} sm={6} xs={12}>
            <ReportCard
              primary={summaryData?.data?.view?.totalViews?.toString()}
              secondary="Total Views"
              color={theme.palette.error.main}
            />
          </Grid>
          <Grid item lg={GRID_COL} sm={6} xs={12}>
            <ReportCard
              primary={summaryData?.data?.clone?.totalClones?.toString()}
              secondary="Total Clones"
              color={theme.palette.success.main}
            />
          </Grid>
        </Grid>
      </Grid>

      <Grid item xs={12}>
        <Grid container spacing={gridSpacing}>
          <Grid item lg={12} xs={12}>
            <Card>
              <CardHeader
                title={
                  <Typography component="div" className="card-header">
                    Filter
                  </Typography>
                }
              />
              <Divider />
              <CardContent>
                <Flex gap="small" wrap>
                  <RangePicker
                    onChange={handleDateChange}
                    value={selectedDateRange}
                  />
                  <Select
                    defaultValue={summaryData?.data?.filterData?.repoId}
                    filterOption={(input, option) =>
                      (option?.label ?? '').toLowerCase().includes(input.toLowerCase())
                    }
                    showSearch
                    placeholder="Select repository"
                    style={{
                      width: 250,
                    }}
                    options={repositoryOpt}
                    onChange={handleRepositoryChange}
                    value={selectedRepository}
                  />
                  <Button type="primary" loading={loading} onClick={handleGenerateClick}>Generate Report</Button>
                  <Button type="default" onClick={handleClearClick}>Clear</Button>
                </Flex>

                <span>Last sync: {lastSync?.syncAtDisplay} (UTC)</span>
              </CardContent>
            </Card>
          </Grid>
        </Grid>
      </Grid>

      <Grid item xs={12}>
        <Grid container spacing={gridSpacing}>
          <Grid item lg={6} xs={12}>
            <Card>
              <CardHeader
                title={
                  <Typography component="div" className="card-header">
                    Traffic Summary of Views
                  </Typography>
                }
              />
              <Divider />
              <CardContent>
                <p>Total views: {`${summaryData?.data?.view?.totalViews}`}</p>
                <p>Unique visitors: {`${summaryData?.data?.view?.uniqueVisitors}`}</p>
                <ResponsiveContainer width="100%" height={500}>
                  <BarChart
                    // width={500}
                    // height={300}
                    data={summaryData?.data?.view?.summaryViews}
                    margin={{
                      top: 5,
                      right: 30,
                      left: 20,
                      bottom: 5,
                    }}
                  >
                    <CartesianGrid strokeDasharray="3 3" />
                    <XAxis dataKey="repoName" />
                    <YAxis />
                    <Tooltip content={<ViewCloneTooltip />} />
                    <Legend />
                    <Bar dataKey="count" barSize={20} fill="#8884d8" />
                  </BarChart>
                </ResponsiveContainer>
              </CardContent>
            </Card>
          </Grid>

          <Grid item lg={6} xs={12}>
            <Card>
              <CardHeader
                title={
                  <Typography component="div" className="card-header">
                    Traffic Summary of Clones
                  </Typography>
                }
              />
              <Divider />
              <CardContent>
                <p>Total clones: {`${summaryData?.data?.clone?.totalClones}`}</p>
                <p>Unique cloners: {`${summaryData?.data?.clone?.uniqueCloners}`}</p>
                <ResponsiveContainer width="100%" height={500}>
                  <BarChart
                    // width={500}
                    // height={300}
                    data={summaryData?.data?.clone?.summaryClones}
                    margin={{
                      top: 5,
                      right: 30,
                      left: 20,
                      bottom: 5,
                    }}
                  >
                    <CartesianGrid strokeDasharray="3 3" />
                    <XAxis dataKey="repoName" />
                    <YAxis />
                    <Tooltip content={<ViewCloneTooltip />} />
                    <Legend />
                    <Bar dataKey="count" barSize={20} fill="#8884d8" />
                  </BarChart>
                </ResponsiveContainer>
              </CardContent>
            </Card>
          </Grid>
        </Grid>
      </Grid>

      <Grid item xs={12}>
        <Grid container spacing={gridSpacing}>
          <Grid item lg={6} xs={12}>
            <Card>
              <CardHeader
                title={
                  <Typography component="div" className="card-header">
                    Traffic Views of {repoFilterd?.repoName}
                  </Typography>
                }
              />
              <Divider />
              <CardContent>
                <p>Total views: {`${summaryData?.data?.filterViews?.totalViews}`}</p>
                <p>Total unique visitors: {`${summaryData?.data?.filterViews?.totalUniqueVisitors}`}</p>
                <hr></hr>
                <p>Data from {repoFilterd?.startDate} to {repoFilterd?.endDate}</p>
                <p>Views: {`${summaryData?.data?.filterViews?.views}`}</p>
                <p>Unique visitors: {`${summaryData?.data?.filterViews?.uniqueVisitors}`}</p>
                <ResponsiveContainer width="100%" height={500}>
                  <LineChart
                    width={500}
                    height={300}
                    data={summaryData?.data?.filterViews?.items}
                    margin={{
                      top: 20,
                      right: 30,
                      left: 20,
                      bottom: 10,
                    }}
                  >
                    <CartesianGrid strokeDasharray="3 3" />
                    <XAxis dataKey="timestamp" height={60} tick={<CustomizedAxisTick />} />
                    <YAxis />
                    <Tooltip />
                    <Legend />
                    <Line type="monotone" dataKey="views" stroke="#8884d8" label={<CustomizedLabel />} />
                    <Line type="monotone" dataKey="uniques" stroke="#82ca9d" />
                  </LineChart>
                </ResponsiveContainer>
              </CardContent>
            </Card>
          </Grid>

          <Grid item lg={6} xs={12}>
            <Card>
              <CardHeader
                title={
                  <Typography component="div" className="card-header">
                    Traffic Clones of {repoFilterd?.repoName}
                  </Typography>
                }
              />
              <Divider />
              <CardContent>
                <p>Total clones: {`${summaryData?.data?.filterClones?.totalClones}`}</p>
                <p>Total unique cloners: {`${summaryData?.data?.filterClones?.totalUniqueCloners}`}</p>
                <hr></hr>
                <p>Data from {repoFilterd?.startDate} to {repoFilterd?.endDate}</p>
                <p>Clones: {`${summaryData?.data?.filterClones?.clones}`}</p>
                <p>Unique cloners: {`${summaryData?.data?.filterClones?.uniqueCloners}`}</p>
                <ResponsiveContainer width="100%" height={500}>
                  <LineChart
                    width={500}
                    height={300}
                    data={summaryData?.data?.filterClones?.items}
                    margin={{
                      top: 20,
                      right: 30,
                      left: 20,
                      bottom: 10,
                    }}
                  >
                    <CartesianGrid strokeDasharray="3 3" />
                    <XAxis dataKey="timestamp" height={60} tick={<CustomizedAxisTick />} />
                    <YAxis />
                    <Tooltip />
                    <Legend />
                    <Line type="monotone" dataKey="clones" stroke="#8884d8" label={<CustomizedLabel />} />
                    <Line type="monotone" dataKey="uniques" stroke="#82ca9d" />
                  </LineChart>
                </ResponsiveContainer>
              </CardContent>
            </Card>
          </Grid>
        </Grid>
      </Grid>
    </Grid>
  );
};

export default Default;
