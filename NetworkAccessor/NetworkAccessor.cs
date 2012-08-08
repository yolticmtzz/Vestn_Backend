﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entity;
using System.Data.Entity;
using System.Data;
using System.Data.Linq;
using Accessor;

namespace Accessor
{
    public class NetworkAccessor
    {
        public Network CreateNetwork(Network network)
        {
            try
            {
                VestnDB db = new VestnDB();
                db.networks.Add(network);

                db.SaveChanges();
                return network;
            }
            catch (Exception ex)
            {
                LogAccessor la = new LogAccessor();
                la.CreateLog(DateTime.Now, "Network Accessor Create Network", ex.StackTrace);
                return null;
            }
        }

        public Network GetNetwork(int networkId)
        {
            Network network;
            VestnDB db = new VestnDB();
            try
            {
                network = db.networks.Where(n => n.id == networkId).Include(n => n.admins).Include(n => n.networkUsers).FirstOrDefault();
            }
            catch(Exception ex)
            {
                LogAccessor la = new LogAccessor();
                la.CreateLog(DateTime.Now, "Network Accessor Get Network", ex.StackTrace);
                return null;
            }
            return network;
        }

        public Network_TopNetwork GetTopNetwork(int networkId)
        {
            Network_TopNetwork network;
            VestnDB db = new VestnDB();
            try
            {
                network = db.networks.OfType<Network_TopNetwork>().Where(n => n.id == networkId)
                    .Include(n => n.subNetworks)
                    .Include(n => n.admins)
                    .Include(n => n.networkUsers)
                    .FirstOrDefault();

                return network;
                
            }
            catch (Exception ex)
            {
                LogAccessor la = new LogAccessor();
                la.CreateLog(DateTime.Now, "Network Accessor Get Network", ex.StackTrace);
                return null;
            }
        }

        public Network_SubNetwork GetSubNetwork(int networkId)
        {
            Network_SubNetwork network;
            VestnDB db = new VestnDB();
            try
            {
                network = db.networks.OfType<Network_SubNetwork>().Where(n => n.id == networkId)
                    .Include(n => n.groups)
                    .Include(n => n.admins)
                    .Include(n => n.networkUsers)
                    .FirstOrDefault();

                return network;

            }
            catch (Exception ex)
            {
                LogAccessor la = new LogAccessor();
                la.CreateLog(DateTime.Now, "Network Accessor Get Network", ex.StackTrace);
                return null;
            }
        }

        public Network_Group GetGroupNetwork(int networkId)
        {
            Network_Group network;
            VestnDB db = new VestnDB();
            try
            {
                network = db.networks.OfType<Network_Group>().Where(n => n.id == networkId)
                    .Include(n => n.admins)
                    .Include(n => n.networkUsers)
                    .FirstOrDefault();

                return network;

            }
            catch (Exception ex)
            {
                LogAccessor la = new LogAccessor();
                la.CreateLog(DateTime.Now, "Network Accessor Get Network", ex.StackTrace);
                return null;
            }
        }

        public Network UpdateNetwork(Network network)
        {
            try
            {
                VestnDB db = new VestnDB();
                var n = new Network { id = network.id };
                n.profileURL = network.profileURL;
                db.SaveChanges();
                return network;
            }
            catch (Exception ex)
            {
                LogAccessor la = new LogAccessor();
                la.CreateLog(DateTime.Now, "Network Accessor Update Network", ex.StackTrace);
                return null;
            }
        }

        public Network UpdateNetworkUrl(Network network)
        {
            try
            {
                VestnDB db = new VestnDB();
                var n = new Network { id = network.id };
                db.networks.Attach(n);
                n.profileURL = network.profileURL;
                db.SaveChanges();
                return network;
            }
            catch (Exception ex)
            {
                LogAccessor la = new LogAccessor();
                la.CreateLog(DateTime.Now, "Network Accessor Update Network", ex.StackTrace);
                return null;
            }
        }

        public Network UpdateNetworkInformation(Network network)
        {
            try
            {
                VestnDB db = new VestnDB();
                var n = new Network { id = network.id };
                db.networks.Attach(n);
                n.name = network.name;
                n.privacy = network.privacy;
                n.description = network.description;
                db.SaveChanges();
                return network;
            }
            catch (Exception ex)
            {
                LogAccessor la = new LogAccessor();
                la.CreateLog(DateTime.Now, "Network Accessor Update Network", ex.StackTrace);
                return null;
            }
        }

        public bool AddSubNetwork(int topNetId, int subNetId)
        {
            try
            {
                VestnDB db = new VestnDB();

                var t = new Network_TopNetwork { id = topNetId };
                var s = new Network_SubNetwork { id = subNetId };
                db.networks.Attach(t);
                db.networks.Attach(s);

                t.subNetworks.Add(s);

                db.SaveChanges();

                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public bool AddGroupNetwork(int subNetId, int groupNetId)
        {
            try
            {
                VestnDB db = new VestnDB();

                var s = new Network_SubNetwork { id = subNetId };
                var g = new Network_Group { id = groupNetId};
                db.networks.Attach(s);
                db.networks.Attach(g);

                s.groups.Add(g);

                db.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool AddAdmin(int networkId, int userId)
        {
            try
            {
                VestnDB db = new VestnDB();

                var n = new Network { id = networkId };
                var u = new User { id = userId };
                db.networks.Attach(n);
                db.users.Attach(u);

                n.admins.Add(u);

                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                LogAccessor la = new LogAccessor();
                la.CreateLog(DateTime.Now, "Network Accessor AddAdmin", ex.StackTrace);
                return false;
            }
        }

        public bool AddNetworkUser(int networkId, int userId)
        {
            try
            {
                VestnDB db = new VestnDB();

                var n = new Network { id = networkId };
                var u = new User { id = userId };
                db.networks.Attach(n);
                db.users.Attach(u);

                n.networkUsers.Add(u);

                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                LogAccessor la = new LogAccessor();
                la.CreateLog(DateTime.Now, "Network Accessor AddAdmin", ex.StackTrace);
                return false;
            }
        }

        public Network GetNetworkByUrl(string networkURL)
        {
            try
            {
                VestnDB db = new VestnDB();

                Network network = db.networks.Where(n => n.profileURL == networkURL).FirstOrDefault();
                return network;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool IsNetworkUrlAvailable(string networkURL)
        {
            try
            {
                VestnDB db = new VestnDB();

                List<Network> query = db.networks.Where(n => n.profileURL == networkURL).ToList();
                if (query == null || query.Count == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch(Exception ex)
            {
                return false;
            }
        }
    }
}
