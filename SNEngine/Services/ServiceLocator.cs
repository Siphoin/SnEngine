﻿using System;
using System.Collections.Generic;

namespace SNEngine.Services
{
    public sealed class ServiceLocator : IResetable
    {
        private static Dictionary<Type, IService> _services;

        public ServiceLocator()
        {
            _services = new Dictionary<Type, IService>();
        }

        public void Initialize()
        {
            IService[] services =
            {
                new InputService(),
                new VaritablesContainerService(),
                new UIService(),
                new InputFormService(),
                new RenderOldDialogueService(),
                new BackgroundService(),
                new DialogueService(),
                new DialogueUIService(),
                new DialogueOnScreenSerice(),
                new CharacterService(),
                new SelectVariantsService(),
                new MainMenuService(),
            };

            foreach (var service in services)
            {
                service.Initialize();

                _services.Add(service.GetType(), service);
            }


        }

        internal T Get<T>() where T : IService
        {
            return (T)_services[typeof(T)];
        }

        public void ResetState()
        {
           foreach (var service in _services.Values)
            {
                if (service is IResetable)
                {
                    var target = service as IResetable;

                    target.ResetState();
                }
            }
        }
    }
}
