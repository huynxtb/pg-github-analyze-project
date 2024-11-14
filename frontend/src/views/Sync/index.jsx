import React, { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import {
  Card,
  CardHeader,
  CardContent,
  Divider,
  Grid,
  Typography
} from '@mui/material';
import Breadcrumb from 'component/Breadcrumb';
import { gridSpacing } from 'config.js';
import { Button, Flex } from 'antd';

const SyncPage = () => {
  const BASE_ADDRESS = import.meta.env.VITE_APP_API_URL;
  const [lastSync, setLastSync] = useState(null);
  const [historySync, setHistorySync] = useState([]);
  const [reSync, setResync] = useState(false);
  const [loading, setLoading] = useState(false);

  console.log('BASE_ADDRESS:', BASE_ADDRESS);
  
  const fetchHistorySync = async () => {
    try {
      const response1 = await fetch(`${BASE_ADDRESS}/traffic/sync/history/last-sync`);
      const response2 = await fetch(`${BASE_ADDRESS}/traffic/sync/history`);
      if (!response1.ok || !response2.ok) {
        throw new Error('Network response was not ok');
      }
      const result1 = await response1.json();
      const result2 = await response2.json();
      setLastSync(result1.data);
      setHistorySync(result2.data);
    } catch (error) {
      console.error('There was a problem with your fetch operation:', error);
    }
  };

  useEffect(() => {
    fetchHistorySync();
  }, [reSync]);

  const handleReSyncClick = async () => {
    setLoading(true);
    try {
      const response = await fetch(`${BASE_ADDRESS}/traffic/sync`, {
        method: 'POST',
      });
      if (!response.ok) {
        throw new Error('Network response was not ok');
      }
      setLoading(false);
      setResync(!reSync);
    } catch (error) {
      setLoading(false);
    } finally {
      setLoading(false);
    }
  }

  return (
    <>
      <Breadcrumb title="Sync Page">
        <Typography component={Link} to="/" variant="subtitle2" color="inherit" className="link-breadcrumb">
          Home
        </Typography>
        <Typography variant="subtitle2" color="primary" className="link-breadcrumb">
          Sync Page
        </Typography>
      </Breadcrumb>

      <Grid container spacing={gridSpacing}>
        <Grid item xs={12}>
          <Grid container spacing={gridSpacing}>
            <Grid item lg={12} xs={12}>
              <Card>
                <CardHeader
                  title={
                    <Typography component="div" className="card-header">
                      Sync Page - Last Sync {lastSync?.syncAtDisplay} (UTC)
                    </Typography>
                  }
                />
                <Divider />
                <CardContent>
                  <Flex gap="small" wrap>
                    <Button type="primary" loading={loading} onClick={handleReSyncClick}>Re-sync</Button>
                  </Flex>
                  <ul>
                    {historySync.map((item, index) => (
                      <li key={index}>{item.syncAtDisplay} - {item.type}</li>
                    ))}
                  </ul>
                </CardContent>
              </Card>
            </Grid>
          </Grid>
        </Grid>
      </Grid>
    </>
  );
};

export default SyncPage;
